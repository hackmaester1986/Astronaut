import { TestBed } from '@angular/core/testing';

import { AtronautDutyService } from './atronaut-duty-service';

describe('AtronautDutyService', () => {
  let service: AtronautDutyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AtronautDutyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
