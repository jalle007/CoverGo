import axios from "axios";
import { Plan } from "@/models/plan";
import { TaskApiRestClientOptions } from "./task-api-rest-client-options";

export class PlansClient {
  constructor(
    public readonly options: TaskApiRestClientOptions) {
  }

  async getAll(): Promise<Plan[]> {
    const response = await axios.get(
      `${this.options.baseUrl}Plans`
    );
    return response.data;
  }
}
