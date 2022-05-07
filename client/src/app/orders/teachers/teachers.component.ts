import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-teachers',
  templateUrl: './teachers.component.html',
  styleUrls: ['./teachers.component.css']
})

export class TeachersComponent {

  title = 'FormArray SetValue & PatchValue Example';
 
  teachersForm: FormGroup;
 
  constructor(private fb: FormBuilder) {
    this.teachersForm = this.fb.group({
      teachers: this.fb.array([]),
    });
    this.patchValue2();
  }
 
 
  /** Teachers */
  teachers(): FormArray {
    return this.teachersForm.get("teachers") as FormArray
  }
 
  newTeacher(): FormGroup {
    return this.fb.group({
      name: '',
      batches: this.fb.array([])
    })
  }
 
 
  addTeacher() {
    this.teachers().push(this.newTeacher());
  }
 
 
  removeTeacher(ti) {
    this.teachers().removeAt(ti);
  }
 
 
  /** batches */
 
  batches(ti): FormArray {
    return this.teachers().at(ti).get("batches") as FormArray
  }
 
 
  newBatch(): FormGroup {
    return this.fb.group({
      name: '',
      students: this.fb.array([])
    })
  }
 
  addBatch(ti: number) {
    this.batches(ti).push(this.newBatch());
  }
 
  removeBatch(ti: number, bi: number) {
    this.batches(ti).removeAt(ti);
  }
 
  /** students */
 
  students(ti, bi): FormArray {
    return this.batches(ti).at(bi).get("students") as FormArray
  }
 
  newStudent(): FormGroup {
    return this.fb.group({
      name: '',
    })
  }
 
  addStudent(ti: number, bi: number) {
    this.students(ti, bi).push(this.newStudent());
  }
 
  removeStudent(ti: number, bi: number, si: number) {
    this.students(ti, bi).removeAt(si);
  }
 
  onSubmit() {
    console.log(this.teachersForm.value);
  }
 
  patchValue2() {
 
    var data = {
      teachers: [
        {
          name: 'Teacher 1', batches: [
            { name: 'Batch No 1', students: [{ name: 'Ramesh' }, { name: 'Suresh' }, { name: 'Naresh' }] },
            { name: 'Batch No 2', students: [{ name: 'Vikas' }, { name: 'Harish' }, { name: 'Lokesh' }] },
          ]
        },
        {
          name: 'Teacher 2', batches: [
            { name: 'Batch No 3', students: [{ name: 'Ramesh 2' }, { name: 'Suresh 3' }, { name: 'Naresh 4' }] },
            { name: 'Batch No 4', students: [{ name: 'Vikas 3' }, { name: 'Harish 3' }, { name: 'Lokesh 4'  }] },
          ]
        }
      ]
    }
    this.clearFormArray();
    
    console.log('data', data);
   
    data.teachers.forEach(t => {
      var teacher: FormGroup = this.newTeacher();
      this.teachers().push(teacher);
   
      t.batches.forEach(b => {
        var batch = this.newBatch();
   
        (teacher.get("batches") as FormArray).push(batch)
   
        b.students.forEach(s => {
          (batch.get("students") as FormArray).push(this.newStudent())
        })
   
      });
    });
   
    this.teachersForm.patchValue(data);
  }
   
   
  clearFormArray() {
   
    //Angular 8 +
    this.teachers().clear();
   
    //older Versions of angualar
    //while (this.teachers().length) {
    //  this.teachers().removeAt(0);
    //}
  }
}