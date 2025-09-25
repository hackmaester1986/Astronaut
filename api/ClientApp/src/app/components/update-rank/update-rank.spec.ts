import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateRank } from './update-rank';

describe('UpdateRank', () => {
  let component: UpdateRank;
  let fixture: ComponentFixture<UpdateRank>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateRank]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateRank);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
