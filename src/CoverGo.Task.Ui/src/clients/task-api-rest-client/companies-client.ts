import axios from "axios";
import { Company } from "@/models/company";
import { TaskApiRestClientOptions } from "./task-api-rest-client-options";

export class CompaniesClient {
  constructor(
    public readonly options: TaskApiRestClientOptions) {
  }

  async getAll(): Promise<Company[]> {
    const response = await axios.get(
      `${this.options.baseUrl}Companies`
    );
    return response.data;
  }
}
