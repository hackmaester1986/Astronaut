import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { AstronautDutyService } from '../../services/atronaut-duty-service';
import { AstronautDetailService } from '../../services/astronaut-detail-service';
import { forkJoin } from 'rxjs';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-astronaut-job',
  imports: [FormsModule],
  templateUrl: './astronaut-job.html',
  styleUrl: './astronaut-job.scss'
})
export class AstronautJob {
  private detailService = inject(AstronautDetailService);

  @Input() showModal = false;
  @Input() person: any;
  @Output() close = new EventEmitter();
  @Output() refresh = new EventEmitter();
  newRank = '';

  openModal() {
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.newRank = '';
    this.close.emit();
  }

  save() {
    if (!this.newRank.trim()) return;
    this.detailService.createAstronautDetail({
      name:this.person!.name,
      rank:this.newRank,
      careerStartDate:new Date().toISOString()
    }).subscribe(result =>{
       this.refresh.emit();
       this.closeModal();

    },
      error => console.log(error)
    )
    
  }
}
