import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './_layout/layout/layout.component';
import { HomeComponent} from './home/home.component';
import { LoginComponent } from './login/login.component';
import { CategoryGroupComponent } from 'src/app/category-group/category-group.component';
import { CategoryComponent } from './category/category.component';
import { BookComponent } from './book/book.component';
import { AuthGuard } from './shared/guards/auth.guard';
import { EditCategoryGroupComponent } from './category-group/edit-category-group/edit-category-group.component';
import { EditCategoryComponent } from './category/edit-category/edit-category.component';
import { EditBookComponent } from './book/edit-book/edit-book.component';

const appRoutes: Routes = [
    // App routes goes here here
    { 
        path: '',
        component: LayoutComponent,
        children: [
           { path: '', redirectTo: 'home', pathMatch: 'full'},
           { path: 'home', component: HomeComponent },
           { path: 'category-group', component: CategoryGroupComponent},
           { path: 'category-group/:id', component: EditCategoryGroupComponent },
           { path: 'category', component: CategoryComponent },
           { path: 'category/:id', component: EditCategoryComponent },
           { path: 'book', component: BookComponent },
           { path: 'book/:id', component: EditBookComponent }
        ],
        canActivate: [ AuthGuard]
    },
    //no layout routes
    { path: 'login', component: LoginComponent},
    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

export const routing = RouterModule.forRoot(appRoutes);