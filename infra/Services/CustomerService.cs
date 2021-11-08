using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using core.Entities;
using core.Entities.Admin;
using core.Entities.Identity;
using core.Interfaces;
using core.ParamsAndDtos;
using infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace infra.Services
{
     public class CustomerService : ICustomerService
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly IMapper _mapper;
          private readonly UserManager<AppUser> _userManager;
          private readonly IUserService _userService;
          private readonly ATSContext _context;
          public CustomerService(IUnitOfWork unitOfWork, IMapper mapper,
               UserManager<AppUser> userManager, IUserService userService,
               ATSContext context)
          {
               _context = context;
               _userService = userService;
               _userManager = userManager;
               _mapper = mapper;
               _unitOfWork = unitOfWork;
          }

          public async Task<ICollection<CustomerDto>> AddCustomers(ICollection<RegisterCustomerDto> dtos)
          {
               var customers = new List<Customer>();
               //using (var scope = new TransactionScope()) 
               //{
                    foreach(var dto in dtos)
                    {
                         var custIndustries = new List<CustomerIndustry>();
                         var custOfficials = new List<CustomerOfficial>();
                         var agencySpecialties = new List<AgencySpecialty>();

                         if (dto.CustomerIndustries != null && dto.CustomerIndustries.Count > 0) 
                         {
                              foreach (var ind in dto.CustomerIndustries)
                              {
                                   custIndustries.Add(new CustomerIndustry { IndustryId = ind.IndustryId });
                              }
                              custIndustries = custIndustries.Count() > 0 ? custIndustries : null;
                         }

                         if (dto.CustomerOfficials != null && dto.CustomerOfficials.Count >0)
                         {
                              //UserManager.CreateAsync never fails, so it will be added after customer official is added succesfully
                              //create identity users for each customer official
                              foreach (var off in dto.CustomerOfficials)
                              {
                                   custOfficials.Add(new CustomerOfficial(off.AppUserId, off.Gender, off.Title,
                                        off.FirstName + " " + off.SecondName + " " + off.FamilyName, off.Designation,
                                             off.PhoneNo, off.Mobile, off.Email, off.ImageUrl, off.LogInCredential));
                              }
                              custOfficials = custOfficials.Count() > 0 ? custOfficials : null;
                         }
                         
                         if (dto.AgencySpecialties != null && dto.AgencySpecialties.Count > 0 )
                         {
                              if(dto.AgencySpecialties!=null && dto.AgencySpecialties.Count() > 0)
                              {
                                   foreach (var sp in dto.AgencySpecialties)
                                   {
                                        agencySpecialties.Add(new AgencySpecialty { IndustryId = sp.IndustryId, ProfessionId = sp.ProfessionId });
                                   }
                              }
                              agencySpecialties = agencySpecialties.Count() > 0 ? agencySpecialties : null;
                         }
                         
                         //add the customer
                         var customer = new Customer(dto.CustomerType, dto.CustomerName, dto.KnownAs, dto.Add,
                              dto.Add2, dto.City, dto.Pin, dto.District, dto.State, dto.Country, dto.Email,
                              dto.Website, dto.Phone, dto.Phone2, custIndustries, custOfficials, agencySpecialties);

                         _unitOfWork.Repository<Customer>().Add(customer);
                         customers.Add(customer);
                    }
                    
                    var result = await _unitOfWork.Complete();

                    //now create identity users for each customer official, and update customer official table
                    //this could have been done before adding the customer official, but if Usermanager.CreateAsync succeeds
                    //(which always succeeds) and customer official insert fails, then we are left with user identity without
                    //corresponding customer officials.  So next time the customer official is to be added, it will nto succeed
                    //because its email Id already exists in AppUser
                    if (result > 0) {
                         foreach(var customer in customers)
                         {
                              foreach(var off in customer.CustomerOfficials)
                              {
                                   if (!string.IsNullOrEmpty(off.Email) && !CheckEmailExistsAsync(off.Email).Result) {
                                        if (off.LogInCredential) 
                                        {
                                             var dtoCust = dtos.Where(x => x.CustomerName.ToLower() == customer.CustomerName.ToLower() && x.City.ToLower() == customer.City.ToLower()).FirstOrDefault();
                                             var dtoOff = dtoCust.CustomerOfficials.Where(x => x.FirstName.ToLower() + " " + x.SecondName.ToLower() + " " + x.FamilyName.ToLower() == off.OfficialName.ToLower())
                                                  .Select(y => new {y.FirstName, y.SecondName, y.FamilyName, y.KnownAs, y.Password, y.Add, y.StreetAdd, y.City, y.Country})
                                                  .FirstOrDefault();
                                             var appuser = new AppUser
                                             {
                                                  UserType = "official",
                                                  Gender = off.Gender,
                                                  DisplayName = off.OfficialName,
                                                  Email = off.Email,
                                                  UserName = off.Email,
                                                  PhoneNumber = off.PhoneNo,
                                                  KnownAs = dtoOff.KnownAs,
                                                  Address = new Address{AddressType="O", Gender=off.Gender,
                                                            FirstName=dtoOff.FirstName,
                                                            Add = dtoOff.Add, StreetAdd=dtoOff.StreetAdd, 
                                                            City=dtoOff.City, Country=dtoOff.Country}
                                             };

                                             var added = await _userManager.CreateAsync(appuser, dtoOff.Password);
                                             if (added.Succeeded) {
                                                  off.AppUserId = appuser.Id;
                                                  _unitOfWork.Repository<CustomerOfficial>().Update(off);
                                             }
                                        }
                                   }
                              }
                         }

                         await _unitOfWork.Complete();
                         return _mapper.Map<ICollection<Customer>, ICollection<CustomerDto>>(customers);
                    }
                    return null;
               //}
          }

     

          public async Task<CustomerDto> AddCustomer(RegisterCustomerDto dto)
          {
               var custIndustries = new List<CustomerIndustry>();
               foreach (var ind in dto.CustomerIndustries)
               {
                    custIndustries.Add(new CustomerIndustry { IndustryId = ind.IndustryId });
               }
               custIndustries = custIndustries.Count() > 0 ? custIndustries : null;

               var custOfficials = new List<CustomerOfficial>();
               foreach (var off in dto.CustomerOfficials)
               {
                    if (off.LogInCredential)
                    {
                         var appuser = new AppUser
                         {
                              UserType = "official",
                              Gender = off.Gender,
                              DisplayName = off.KnownAs,
                              Email = off.Email,
                              UserName = off.Email,
                              PhoneNumber = off.PhoneNo,
                              Address = new Address{AddressType="O", Gender=off.Gender,
                                        FirstName=off.FirstName + " " + off.SecondName + " " + off.FamilyName,
                                        Add = dto.Add, StreetAdd=dto.Add2, City=dto.City, Country=dto.Country}
                         };

                         var added = await _userManager.CreateAsync(appuser, off.Password);
                         /* if (added.Succeeded)
                         {
                              var user = await _userManager.FindByEmailAsync(off.Email);
                              off.AppUserId =  int.Parse(user.Id.value);
                         }
                         */
                    }

                    custOfficials.Add(new CustomerOfficial(off.AppUserId, off.Gender, off.Title,
                         off.FirstName + " " + off.SecondName + " " + off.FamilyName, off.Designation,
                              off.PhoneNo, off.Mobile, off.Email, off.ImageUrl, off.LogInCredential));
               }
               custOfficials = custOfficials.Count() > 0 ? custOfficials : null;

               var agencySpecialties = new List<AgencySpecialty>();
               if(dto.AgencySpecialties!=null && dto.AgencySpecialties.Count() > 0)
               {
                    foreach (var sp in dto.AgencySpecialties)
                    {
                         agencySpecialties.Add(new AgencySpecialty { IndustryId = sp.IndustryId, ProfessionId = sp.ProfessionId });
                    }
               }
               agencySpecialties = agencySpecialties.Count() > 0 ? agencySpecialties : null;

               var customer = new Customer(dto.CustomerType, dto.CustomerName, dto.KnownAs, dto.Add,
                    dto.Add2, dto.City, dto.Pin, dto.District, dto.State, dto.Country, dto.Email,
                    dto.Website, dto.Phone, dto.Phone2, custIndustries, custOfficials, agencySpecialties);

               _unitOfWork.Repository<Customer>().Add(customer);

               var result = await _unitOfWork.Complete();

               if (result <= 0) return null;

               return _mapper.Map<Customer, CustomerDto>(customer);
          }

          public Task<bool> CustomerExistsByIdAsync(int id)
          {
               throw new System.NotImplementedException();
          }

          public void DeleteCustomer(Customer customer)
          {
               throw new System.NotImplementedException();
          }

          public void EditCustomer(Customer customer)
          {
               throw new System.NotImplementedException();
          }

          public Task<CustomerDto> GetCustomerByIdAsync(int id)
          {
               throw new System.NotImplementedException();
          }

          public Task<CustomerDto> GetCustomerByUserNameAsync(string username)
          {
               throw new System.NotImplementedException();
          }

          public async Task<ICollection<CustomerIdAndNameDto>> GetCustomerIdAndName(string customerType)
          {
               var qry = await _context.Customers
                    .Where(x => x.CustomerType.ToLower() == customerType)
                    .Select(x => new {x.Id, x.CustomerName})
                    .OrderBy(x => x.CustomerName)
                    .ToListAsync();
               var lst = new List<CustomerIdAndNameDto>();
               foreach(var item in qry)
               {
                    lst.Add(new CustomerIdAndNameDto{Id=item.Id, CustomerName=item.CustomerName});
               }
               return lst;
          }

          public Task<ICollection<CustomerIdAndNameDto>> GetCustomerIdAndNames(ICollection<int> customerIds)
          {
               throw new System.NotImplementedException();
          }

          public Task<string> GetCustomerNameFromId(int Id)
          {
               throw new System.NotImplementedException();
          }

          public Task<ICollection<CustomerDto>> GetCustomersAsync(string userType)
          {
               throw new System.NotImplementedException();
          }

          public Task<ICollection<CustomerDto>> GetCustomersPaginatedAsync(CustomerParams custParam)
          {
               throw new System.NotImplementedException();
          }

          public void UpdateCustomer(Customer customer)
          {
               throw new System.NotImplementedException();
          }
          private async Task<bool> CheckEmailExistsAsync(string email)
          {
               return await _userManager.FindByEmailAsync(email) != null;
          }
    }
}