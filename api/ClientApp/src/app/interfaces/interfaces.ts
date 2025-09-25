/** Match your API DTOs */
export interface Person {
  id: number;
  name: string;
  //astronautDetail?: any;
  //astronautDuties?: AstronautDuty[]
}

export interface AstronautDuty {
  id?: number;
  personId: number;
  rank: string;
  dutyTitle: string;
  dutyStartDate: string;         // ISO date string (e.g. "2025-09-23")
  dutyEndDate?: string | null;   // null or ISO string
}

export interface CreateAstronautDutyRequest {
  name: string;
  dutyDescription:string;
}

export interface CreateAstronautDetailRequest {
  name: string;
  currentRank: string;
  currentDutyTitle: string;
}
export interface CreateNewAstronautRequest {
  name: string;
  rank: string;
  careerStartDate: string;
}
export interface AstronautDetailDto{
  name:string;
  currentRank:string;
  currentDutyTitle:string;
  careerStartDate:Date;
  careerEndDate:Date | null;
}