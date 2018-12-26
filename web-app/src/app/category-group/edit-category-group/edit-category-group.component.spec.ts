import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCategoryGroupComponent } from './edit-category-group.component';

describe('EditCategoryGroupComponent', () => {
  let component: EditCategoryGroupComponent;
  let fixture: ComponentFixture<EditCategoryGroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditCategoryGroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditCategoryGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
