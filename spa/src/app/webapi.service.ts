import { inject, Injectable } from "@angular/core";
import { environment } from "src/environments/environment.development";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: "root",
})
export class WebApiService {
  constructor() {}

  private http = inject(HttpClient);
  private apiUrl = environment.apiURL + "/weatherforecast";

  get(): Observable<any> {
    return this.http.get(this.apiUrl);
  }
}
