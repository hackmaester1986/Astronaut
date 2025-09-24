import { TestBed } from '@angular/core/testing';

import { AstronautDetailService } from './astronaut-detail-service';

describe('AstronautDetailService', () => {
  let service: AstronautDetailService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AstronautDetailService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
