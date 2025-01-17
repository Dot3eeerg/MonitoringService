import { inject, Injectable } from "@angular/core";
import { environment } from "src/environments/environment.development";
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { Device } from "../models/device.interface";

@Injectable({
  providedIn: "root",
})
export class WebApiService {
  constructor(private http: HttpClient) {}

  private apiUrl = environment.apiURL + "/api/device";

  getAllDevices(): Observable<Device[]> {
    return this.http.get<Device[]>(this.apiUrl);
  }

  getSingleDevice(route: string): Observable<Device> {
    return this.http.get<Device>(this.apiUrl + route);
  }

  deleteSession(deviceId: string, sessionId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${deviceId}/${sessionId}`);
  }
}
