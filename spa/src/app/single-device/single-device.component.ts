import { Component, inject, OnInit } from "@angular/core";
import { WebApiService } from "../services/webapi.service";
import { Device } from "../models/device.interface";

interface SearchQuery {
  deviceId: string;
  sessionName: string;
}

@Component({
  selector: "app-single-device",
  templateUrl: "./single-device.component.html",
  styleUrls: ["./single-device.component.less"],
})
export class SingleDeviceComponent {
  searchQuery: SearchQuery = {
    deviceId: "",
    sessionName: "",
  };

  device: Device | null = null;
  loading = false;
  error: string | null = null;

  private webApiService = inject(WebApiService);

  onSubmit() {
    if (!this.searchQuery.deviceId) {
      this.error = "Please enter a Device ID";
      return;
    }

    this.loading = true;
    this.error = null;

    const route = this.searchQuery.sessionName
      ? `/${this.searchQuery.deviceId}` + `/${this.searchQuery.sessionName}`
      : `/${this.searchQuery.deviceId}`;

    this.webApiService.getSingleDevice(route).subscribe({
      next: (data) => {
        this.device = data;
        this.loading = false;
      },
      error: (error) => {
        console.error("Error fetching device:", error);
        this.error = "Failed to load device data";
        this.loading = false;
      },
    });
  }

  resetForm() {
    this.searchQuery = {
      deviceId: "",
      sessionName: "",
    };
    this.device = null;
    this.error = null;
  }
}
