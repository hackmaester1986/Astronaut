import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AstronautPage } from './astronaut-page';

describe('AstronautPage', () => {
  let component: AstronautPage;
  let fixture: ComponentFixture<AstronautPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AstronautPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AstronautPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
