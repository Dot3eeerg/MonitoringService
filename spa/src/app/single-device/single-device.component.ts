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
      this.error = "Не введён ID устройства";
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
        console.error("Ошибка при получении устройства:", error);
        this.error = "Не получилось загрузить информацию об устройстве";
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

  hasSessions() {
    return this.device?.sessions && this.device.sessions.length > 0;
  }

  deleteSession(deviceId: string, sessionId: string) {
    this.loading = true;
    this.webApiService.deleteSession(deviceId, sessionId).subscribe({
      next: () => {
        // Перезагружаем информацию об устройстве
        this.onSubmit();
      },
      error: (error) => {
        console.error("Ошибка при удалении сессии:", error);
        this.error = "Не удалось удалить сессию";
        this.loading = false;
      },
    });
  }
}
