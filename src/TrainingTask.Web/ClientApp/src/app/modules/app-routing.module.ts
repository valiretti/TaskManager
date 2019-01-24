import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EmployeesComponent } from '../components/employees/employees.component';
import { TasksComponent } from '../components/tasks/tasks.component';
import { ProjectsComponent } from '../components/projects/projects.component';
import { NotFoundComponent } from '../components/not-found/not-found.component';


const routes: Routes = [
  { path: '', redirectTo: '/employees', pathMatch: 'full' },
  { path: 'employees', component: EmployeesComponent },
  { path: 'tasks', component: TasksComponent },
  { path: 'projects', component: ProjectsComponent },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
