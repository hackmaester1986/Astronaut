import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe, NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PersonService } from '../../services/person-service';
import { AstronautDutyService } from '../../services/atronaut-duty-service';
import { AstronautDuty ,Person} from '../../interfaces/interfaces';

@Component({
  selector: 'app-astronaut-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './astronaut-page.html',
  styleUrls: ['./astronaut-page.scss']
})
export class AstronautDutyViewerComponent implements OnInit {
  people: Person[] = [];
  selectedName = '';
  person: Person | null = null;
  duties: AstronautDuty[] = [];
  loading = false;
  error = '';

  constructor(
    private personService: PersonService,
    private dutyService: AstronautDutyService
  ) {}

  ngOnInit(): void {
    this.loadPeople();
  }

  loadPeople(): void {
    this.error = '';
    this.personService.getPeople().subscribe({
      next: (res: any) => {
        //const allPeople = Array.isArray(res) ? res as Person[] : (res?.people ?? [])
        const allPeople = res as Person[];
        this.people = allPeople.filter(p => p.astronautDetail != null);
        console.log(this.people);
      },
      error: (e) => this.error = 'Failed to load astronauts list.'
    });
  }

  onSelectName(): void {
    if (!this.selectedName) { this.person = null; this.duties = []; return; }
    var any = this.people.filter(p => p.name === this.selectedName);
    if(any.length > 0){
      this.person = any[0];
      this.duties = this.person?.astronautDuties ?? [];
    }
    else{
      this.person = null; this.duties = []; return;
    }
    //console.log(this.person);
    /*this.dutyService.getByPersonName(this.selectedName).subscribe({
      next: (res: GetAstronautDutiesByNameResult) => {
        this.person = res?.person ?? null;
        this.duties = res?.astronautDuties ?? [];
        this.loading = false;
      },
      error: () => { this.error = 'Failed to load astronaut duties.'; this.loading = false; }
    });*/
  }
}
