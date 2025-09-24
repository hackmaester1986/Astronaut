/** Match your API DTOs */
export interface Person {
  id: number;
  name: string;
  astronautDetail?: any;
  astronautDuties?: AstronautDuty[]
}

export interface AstronautDuty {
  id?: number;
  personId: number;
  rank: string;
  dutyTitle: string;
  dutyStartDate: string;         // ISO date string (e.g. "2025-09-23")
  dutyEndDate?: string | null;   // null or ISO string
}

/*export interface PersonAstronaut {
  personId: number;
  name: string;
  currentRank?: string;
  currentDutyTitle?: string;
  careerStartDate?: string | null;
  careerEndDate?: string | null;
  astronautDetail?: any;
  astronautDuties?: AstronautDuty[]
}

export interface BaseResponse {
  success: boolean;
  message?: string;
  responseCode?: number;
}

export interface GetAstronautDutiesByNameResult extends BaseResponse {
  person: PersonAstronaut | null;
  astronautDuties: AstronautDuty[];
}*/

export interface CreateAstronautDetailRequest {
  name: string;
  rank: string;
  title: string;
}