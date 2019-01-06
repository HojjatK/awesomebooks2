import { CommonModule } from '@angular/common';
import {NgModule} from '@angular/core';
import { MatButtonModule, MatCheckboxModule, MatTableModule, MatPaginatorModule, MatTooltipModule,
         MatFormFieldModule, MatInputModule, MatSidenavModule, MatProgressSpinnerModule,
         MatSortModule, MatCardModule, MatProgressBarModule, MatSelectModule, MatDialogModule } from '@angular/material';

import { ScrollDispatchModule } from '@angular/cdk/scrolling';
import { GridComponent } from './shared/components/grid/grid.component';

@NgModule({
  declarations: [
    GridComponent,
   ],
  imports: [ 
    CommonModule,
    MatButtonModule,
    MatCheckboxModule,
    MatTableModule,
    MatPaginatorModule,
    MatTooltipModule,
    MatFormFieldModule,
    MatInputModule,
    MatSidenavModule,
    MatProgressSpinnerModule,
    MatSortModule,
    MatCardModule,
    MatProgressBarModule,
    MatSelectModule,
    MatDialogModule,
    ScrollDispatchModule
  ],
  exports: [ 
    CommonModule,
    MatButtonModule,
    MatCheckboxModule,
    MatTableModule,
    MatPaginatorModule,
    MatTooltipModule,
    MatFormFieldModule,
    MatInputModule,
    MatSidenavModule,
    MatProgressSpinnerModule,
    MatCardModule,
    MatProgressBarModule,
    MatSortModule,
    MatSelectModule,
    MatDialogModule,
    ScrollDispatchModule,
    GridComponent],
})
export class AppMaterialModule { 
}