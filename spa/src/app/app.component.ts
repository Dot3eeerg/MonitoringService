import { Component, inject, OnInit } from "@angular/core";
import { WebApiService } from "./webapi.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.less"],
})
export class AppComponent implements OnInit {
  title = "spa";
  webApis: any[] = [];
  loading = false;
  error: string | null = null;

  private webApiService = inject(WebApiService);

  ngOnInit() {
    this.loadData();
  }

  private loadData() {
    this.loading = true;
    this.webApiService.get().subscribe({
      next: (data) => {
        this.webApis = data;
        this.loading = false;
      },
      error: (error) => {
        console.error("Error fetching data:", error);
        this.error = "Failed to load data";
        this.loading = false;
      },
    });
  }

  constructor() {}
}
