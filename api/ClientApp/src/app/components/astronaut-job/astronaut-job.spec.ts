import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AstronautJob } from './astronaut-job';

describe('AstronautJob', () => {
  let component: AstronautJob;
  let fixture: ComponentFixture<AstronautJob>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AstronautJob]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AstronautJob);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
