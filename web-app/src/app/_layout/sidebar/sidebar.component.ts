import { Component, OnInit } from '@angular/core';
import { MessengerService } from 'src/app/shared/services/messenger/messenger.service';
import { ElementRef } from '@angular/core/src/linker/element_ref';
import { AuthService} from 'src/app/shared/services/auth/auth.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  menuItems : any[] = [
    {
      route: '/home',
      title: 'Home',
      icon: 'home'
    },
    {
      route: '/category-group',
      title: 'Category Groups',
      icon: 'list-rich'
    },
    {
      route: '/category',
      title: 'Categories',
      icon: 'grid-three-up'
    },
    {
      route: '/book',
      title: 'Books',
      icon: 'book'
    },
    {
      route: null,
      title: 'Logout',
      icon: 'account-logout'
    }
  ];
  
  constructor(
    private messenger: MessengerService,
    private authService: AuthService) {
  }

  public get currentUser() {
    if (this.authService.currentUser != null) {
      return this.authService.currentUser.username;
    }
    return '';
  }

  ngOnInit() {
  }

  activeIndex : number = 0;
  closeSideMenu(index: number) {
    this.activeIndex = index;
    let menuItem = this.menuItems[index];
    if (menuItem.title === 'Logout') {
      this.authService.logout();
    }
    else {
      this.messenger.raise({name:'sidenav-toggle', payload:null });
    }
  }
}
