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
import { IdBreadcrumbsResolver } from './shared/services/breadcrumb/IdBreadcrumbsResolver';

const appRoutes: Routes = [
    // App routes goes here here
    { 
        path: '',
        component: LayoutComponent,
        data: { breadcrumbs: false },
        children: [
           { path: '', redirectTo: 'home', pathMatch: 'full'},
           { path: 'home', component: HomeComponent, data: { breadcrumbs: true } },
           { path: 'category-group', component: CategoryGroupComponent, data: { breadcrumbs: true, text: 'Category Group' } },
           { path: 'category-group/:id', component: EditCategoryGroupComponent, data: { breadcrumbs: IdBreadcrumbsResolver }  },
           { path: 'category', component: CategoryComponent, data: { breadcrumbs: true, text: 'Category'}  },
           { path: 'category/:id', component: EditCategoryComponent, data: { breadcrumbs: IdBreadcrumbsResolver }  },
           { path: 'book', component: BookComponent, data: {breadcrumbs: true, text: 'Book'}  },
           { path: 'book/:id', component: EditBookComponent, data: { breadcrumbs: IdBreadcrumbsResolver } }
        ],
        canActivate: [ AuthGuard]
    },
    //no layout routes
    { path: 'login', component: LoginComponent},
    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

export const routing = RouterModule.forRoot(appRoutes);