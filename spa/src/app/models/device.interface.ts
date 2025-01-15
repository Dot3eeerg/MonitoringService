export interface Session {
  name: string;
  startTime: string;
  endTime: string;
  version: string;
}

export interface Device {
  id: string;
  sessions: Session[];
}
