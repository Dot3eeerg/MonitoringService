import { Component, inject, OnInit } from "@angular/core";
import { WebApiService } from "../services/webapi.service";
import { Device } from "../models/device.interface";

@Component({
  selector: "app-all-devices",
  templateUrl: "./all-devices.component.html",
  styleUrls: ["./all-devices.component.less"],
})
export class AllDevicesComponent implements OnInit {
  title = "Monitoring SPA";
  devices: Device[] = [];
  loading = false;
  error: string | null = null;

  private webApiService = inject(WebApiService);

  ngOnInit() {
    this.loadData();
  }

  private loadData() {
    this.loading = true;
    this.webApiService.getAllDevices().subscribe({
      next: (data) => {
        this.devices = data;
        this.loading = false;
      },
      error: (error) => {
        console.error("Error fetching data:", error);
        this.error = "Failed to load data";
        this.loading = false;
      },
    });
  }
}
