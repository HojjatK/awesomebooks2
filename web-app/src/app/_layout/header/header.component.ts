import { Component} from '@angular/core';
import { MessengerService } from './../../shared/services/messenger/messenger.service';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  constructor(private messenger: MessengerService) {
    this.messenger.event$.subscribe(e => {
      if (e.name == 'sidenav-toggle') {
        this.menuOpen = !this.menuOpen;
      }
    });
  }
  public searchText : string = '';
  private menuOpen : boolean = false;

  menuClick() {  
    this.messenger.raise( {
      name: 'sidenav-toggle',
      payload: null
    });
  }

  search() {
    alert('serach: ' + this.searchText);
  }
}
