import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { HttpClientModule } from "@angular/common/http";
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";

import { AppComponent } from "./app.component";
import { AllDevicesComponent } from "./all-devices/all-devices.component";
import { SingleDeviceComponent } from "./single-device/single-device.component";

@NgModule({
  declarations: [AppComponent, AllDevicesComponent, SingleDeviceComponent],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: "single-device", component: SingleDeviceComponent },
      { path: "all-devices", component: AllDevicesComponent },
      { path: "", redirectTo: "/single-device", pathMatch: "full" },
    ]),
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
