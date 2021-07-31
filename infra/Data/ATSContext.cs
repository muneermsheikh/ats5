using System.Reflection;
using core.Entities;
using core.Entities.Admin;
using core.Entities.HR;
using core.Entities.Identity;
using core.Entities.MasterEntities;
using core.Entities.Message;
using core.Entities.Orders;
using core.Entities.Process;
using core.Entities.Tasks;
using core.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace infra.Data
{
    public class ATSContext: DbContext
    {
        public ATSContext(DbContextOptions<ATSContext> options) : base(options)
          {
          }

          protected ATSContext()
          {
          }

    
          //general
          public DbSet<Customer> Customers {get; set;}
          public DbSet<CustomerIndustry> CustomerIndustries {get; set;}
          public DbSet<CustomerOfficial> CustomerOfficials {get; set;}
          public DbSet<AgencySpecialty> AgencySpecialties {get; set;}
          public DbSet<Message> Messages {get; set;}
     //HR
          public DbSet<ChecklistHR> ChecklistHRs {get; set;}
          public DbSet<CandidateAssessment> CandidateAssessments {get; set;}
          public DbSet<CandidateAssessmentItem> CandidateAssessmentItems {get; set;}
          public DbSet<ChecklistItemHR> CheckListItemHRs {get; set;}
          public DbSet<CVRef> CVRefs {get; set;}
          public DbSet<CVRefRestriction> CVRefRestrictions {get; set;}
          public DbSet<SelectionDecision> SelectionDecisions {get; set;}
          public DbSet<Employment> Employments {get; set;}
     
       // masters
          //public DbSet<core.Entities.Users.Address> Addresses {get; set;}
          public DbSet<AssessmentQBank> AssessmentQsBank { get; set; }
          public DbSet<Category> Categories {get; set;}
          public DbSet<ChecklistHRData> ChecklistHRDatas {get; set;}
          //public DbSet<DeployStage> DeployStages {get; set;}
          public DbSet<Employee> Employees {get; set;}
          public DbSet<EmployeeQualification> EmployeeQualifications {get; set;}
          public DbSet<EmployeeHRSkill> EmployeeHRSkills {get; set;}
          public DbSet<EmployeeOtherSkill> EmployeeOtherSkills {get; set;}
          public DbSet<EmployeePhone> EmployeePhones {get; set;}
          public DbSet<Industry> Industries {get; set;}
          public DbSet<JobDescription> JobDescriptions {get; set;}
          public DbSet<Qualification> Qualifications {get; set;}
          public DbSet<Remuneration> Remunerations {get; set;}
          public DbSet<ReviewItemStatus> ReviewItemStatuses {get; set;}
          public DbSet<ReviewStatus> ReviewStatuses {get; set;}
          public DbSet<SkillData> SkillDatas {get; set;}
          public DbSet<SelectionStatus> SelectionStatuses {get; set;}
          
          
     ///orders
          
          public DbSet<ContractReview> ContractReviews {get; set;}
          public DbSet<ContractReviewItem> ContractReviewItems {get; set;}
          public DbSet<Order> Orders {get; set;}
          public DbSet<OrderItem> OrderItems {get; set;}
          public DbSet<OrderItemAssessment> OrderItemAssessments {get; set;}
          public DbSet<OrderItemAssessmentQ> OrderItemAssessmentQs {get; set;}
          public DbSet<ReviewItemData> ReviewItemDatas{get; set;}

      //Process
          public DbSet<Deploy> Deploys {get; set;}
          public DbSet<DeployStatus> DeployStatus {get; set;}
          
     //Tasks
          public DbSet<Task> Tasks {get; set;}
          public DbSet<TaskItem> TaskItems {get; set;}
         

     //users

          public DbSet<Candidate> Candidates {get; set;}
          public DbSet<Photo> Photos {get; set;}
          //public DbSet<core.Entities.Users.Address> UserAddresses {get; set;}
          public DbSet<UserExp> UserExps {get; set;}
          //public DbSet<UserLike> UserLikes {get; set;}
          public DbSet<UserPassport> UserPassports {get; set;}
          public DbSet<UserPhone> UserPhones {get; set;}
          public DbSet<UserProfession> UserProfessions{get; set;}
          public DbSet<UserQualification> UserQualifications {get; set;}
          public DbSet<UserAttachment> UserAttachments {get; set;}
     // APPIDENTITYDBCONTEXT
          //public DbSet<UserLike> Likes { get; set; }
         public DbSet<Group> Groups { get; set; }
          public DbSet<Connection> Connections { get; set; }
    

          //required for migrations
          protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
               base.OnModelCreating(modelBuilder);
               modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
          }

    }
}