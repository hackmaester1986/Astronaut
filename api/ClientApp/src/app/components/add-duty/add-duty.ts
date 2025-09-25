import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AstronautDutyViewerComponent } from '../astronaut-page/astronaut-page';
import { AstronautDutyService } from '../../services/atronaut-duty-service';
import { Person } from '../../interfaces/interfaces';
import { forkJoin } from 'rxjs';
import { AstronautDetailService } from '../../services/astronaut-detail-service';

@Component({
  selector: 'app-add-duty',
  templateUrl: './add-duty.html',
  standalone:true,
  imports:[FormsModule],
  styleUrls: ['./add-duty.scss']
})
export class AddDutyComponent {
  private astronautDutyService = inject(AstronautDutyService);
  private detailService = inject(AstronautDetailService);

  @Input() showModal = false;
  @Input() person: any;
  @Input() duties: any;
  @Output() close = new EventEmitter();
  @Output() refresh = new EventEmitter();
  newDutyTitle = '';

  openModal() {
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.newDutyTitle = '';
    this.close.emit();
  }

  save() {
    if (!this.newDutyTitle.trim()) return;
    
    this.astronautDutyService.createDuty({
      name:this.person!.name,
      dutyDescription:this.newDutyTitle
    }).subscribe(result =>{
      console.log(result);
      forkJoin({detail: this.detailService.getDetailByName(this.person.name),duties:this.astronautDutyService.getByPersonName(this.person.name)}).subscribe(
        ({detail,duties}) =>{
          var data = {
            details:detail,
            duty:duties
          }
          this.refresh.emit(data);
        },
        error => {
          console.log(error);
        }
      )
    },
      error => console.log(error)
    )
    
    this.closeModal();
  }
}