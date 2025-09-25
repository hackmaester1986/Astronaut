import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddDuty } from './add-duty';

describe('AddDuty', () => {
  let component: AddDuty;
  let fixture: ComponentFixture<AddDuty>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddDuty]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddDuty);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
