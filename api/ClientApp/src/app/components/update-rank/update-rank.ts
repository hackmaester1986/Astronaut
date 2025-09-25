import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { AstronautDutyService } from '../../services/atronaut-duty-service';
import { AstronautDetailService } from '../../services/astronaut-detail-service';
import { forkJoin } from 'rxjs';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-update-rank',
  imports: [FormsModule],
  templateUrl: './update-rank.html',
  styleUrl: './update-rank.scss'
})
export class UpdateRank {
  private detailService = inject(AstronautDetailService);

  @Input() showModal = false;
  @Input() person: any;
  @Input() detail: any;
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

    this.detailService.updateAstronautDetail({
      name:this.person!.name,
      currentRank:this.newRank,
      currentDutyTitle:this.detail.currentDutyTitle
    }).subscribe(result =>{
       this.refresh.emit(this.newRank);
       this.closeModal();

    },
      error => console.log(error)
    )
    
  }
}
