import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe, NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PersonService } from '../../services/person-service';
import { AstronautDutyService } from '../../services/atronaut-duty-service';
import { AstronautDetailDto, AstronautDuty ,Person} from '../../interfaces/interfaces';
import { AddDutyComponent } from '../add-duty/add-duty';
import { forkJoin } from 'rxjs';
import { AstronautDetailService } from '../../services/astronaut-detail-service';
import { UpdateRank } from "../update-rank/update-rank";
import { AstronautJob } from "../astronaut-job/astronaut-job";

@Component({
  selector: 'app-astronaut-page',
  standalone: true,
  imports: [CommonModule, FormsModule, AddDutyComponent, UpdateRank, AstronautJob],
  templateUrl: './astronaut-page.html',
  styleUrls: ['./astronaut-page.scss']
})
export class AstronautDutyViewerComponent implements OnInit {
  people: Person[] = [];
  selectedName = '';
  person: Person | null = null;
  duties: AstronautDuty[] = [];
  detail:AstronautDetailDto | null = null;
  loading = false;
  error = '';
  showModal = false;
  showModalRank = false;
  showModalJob = false;

  constructor(
    private personService: PersonService,
    private dutyService: AstronautDutyService,
    private detailService: AstronautDetailService
  ) {}

  ngOnInit(): void {
    this.loadPeople();
  }

  loadPeople(): void {
    this.error = '';
    this.personService.getPeople().subscribe({
      next: (res: any) => {
        const allPeople = res as Person[];
        this.people = res as Person[];
      },
      error: (e) => console.log(e)
    });
  }

  setDuties(data: any){
    this.duties = data.duty;
    this.detail = data.details;
  }

  setRank(data: any){
    this.detail!.currentRank = data;
  }

  setDetail(){
    this.detailService.getDetailByName(this.person!.name).subscribe(detail=>{
      this.detail = detail;
    },
     error => console.log(error)
    )
  }

  onSelectName(): void {
    if (!this.selectedName) { this.person = null; this.duties = []; return; }
    var any = this.people.filter(p => p.name === this.selectedName);
    if(any.length > 0){
      this.person = any[0];
      this.detailService.getDetailByName(this.person.name).subscribe(detail=>{
          this.detail = detail;
          console.log(this.detail);
          if(this.detail){
            this.dutyService.getByPersonName(this.person!.name).subscribe(duties =>{
              this.duties = duties;
            },
              error => {
                this.duties = [];
                console.log(error)
              }
            )
          }
          else{
            this.detail = null;
            this.duties = [];
          }
        },
        error => {
          this.detail = null;
          this.duties = [];
          console.log(error);
        }
      )
    }
    else{
      this.person = null; this.duties = []; return;
    }
  }

  openModal(){
    this.showModal = true;
  }
  openModalRank(){
    this.showModalRank = true;
  }
  openModalJob(){
    this.showModalJob = true;
  }
}
