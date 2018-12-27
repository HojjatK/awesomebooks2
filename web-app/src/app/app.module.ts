import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http'; 
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppMaterialModule } from 'src/app/app.material.module';
import { ChartsModule } from 'ng2-charts';

import { PopoverModule } from 'ngx-bootstrap/popover';
import { AlertModule } from 'ngx-bootstrap';
import { CarouselModule } from 'ngx-bootstrap';

import { AppComponent } from './app.component';
import { FileUploadComponent } from 'src/app/shared/components/file-upload/file-upload.component';
import { CardComponent } from 'src/app/shared/components/card/card.component';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { NotificationComponent } from 'src/app/shared/components/notification/notification.component';
import { PageComponent } from './shared/components/page/page.component';
import { LayoutComponent } from './_layout/layout/layout.component';
import { HeaderComponent } from './_layout/header/header.component';
import { HomeComponent } from './home/home.component';

import { routing } from './app.routing';
import { LoginComponent } from './login/login.component';
import { SignInComponent } from './login/sign-in/sign-in.component';
import { SignUpComponent } from './login/sign-up/sign-up.component';
import { CategoryComponent } from './category/category.component';
import { EditCategoryComponent } from './category/edit-category/edit-category.component';
import { CategoryGroupComponent } from './category-group/category-group.component';
import { EditCategoryGroupComponent } from './category-group/edit-category-group/edit-category-group.component';
import { BookComponent } from './book/book.component';
import { EditBookComponent } from './book/edit-book/edit-book.component';
import { SidebarComponent } from './_layout/sidebar/sidebar.component';
import { MenuPopoverComponent } from './shared/components/menu-popover/menu-popover.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthService } from 'src/app/shared/services/auth/auth.service';
import { MessengerService } from 'src/app/shared/services/messenger/messenger.service';

import { AuthGuard } from 'src/app/shared/guards/auth.guard';
import { HttpHeadersInterceptor } from './shared/services/interceptors/http-header.interceptor.';
import { HttpResponseInterceptor } from './shared/services/interceptors/http-response.interceptor';
import { HttpLoadingInterceptor } from './shared/services/interceptors/http-loading.interceptor';
import { LoggerService } from './shared/services/logger/logger.service';
import { HttpLoadingService } from './shared/services/http-loading/http-loading.service';
import { CategoryGroupService } from './shared/services/category-group/category-group.service';
import { CategoryService } from './shared/services/category/category.service';
import { BookService } from './shared/services/book/book.service';
import { FileService } from './shared/services/file/file.service';

@NgModule({
  declarations: [
    AppComponent,
    FileUploadComponent,
    CardComponent,
    ConfirmDialogComponent,
    NotificationComponent,
    PageComponent,
    LayoutComponent,
    HeaderComponent,
    HomeComponent,
    LoginComponent,
    SignInComponent,
    SignUpComponent,
    CategoryGroupComponent,
    EditCategoryGroupComponent,
    CategoryComponent,
    EditCategoryComponent,
    BookComponent,
    EditBookComponent,
    SidebarComponent,
    MenuPopoverComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    AlertModule.forRoot(),
    PopoverModule.forRoot(),
    CarouselModule.forRoot(),
    ChartsModule,
    routing,
    AppMaterialModule
  ],
  providers: [
    AuthGuard,
    AuthService,
    LoggerService,
    CategoryGroupService,
    CategoryService,
    BookService,
    FileService,
    MessengerService,
    HttpLoadingService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpHeadersInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpResponseInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpLoadingInterceptor,
      multi: true,
    }
  ],
  bootstrap: [AppComponent],
  entryComponents: [ConfirmDialogComponent]
})
export class AppModule { }
