import { HttpClient } from '@angular/common/http';
import {  Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Story } from '../models/story';

@Injectable({
  providedIn: 'root'
})
export class StoryService {

  constructor(
    private _http: HttpClient) { }


  getStoriesData(url: string): Observable<Story[]> {
    return this._http.get<Story[]>(url);
  }
}
