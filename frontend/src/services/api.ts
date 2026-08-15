import type {Result} from "../models/Result.tsx";
import type {ResultQuery} from "../models/ResultQuery.ts";
import type {Value} from "../models/Value.ts";

const API_URL = "/api";

export async function getResults(
    query: ResultQuery
): Promise<Result[]> {

    const params = new URLSearchParams(
        Object.entries(query)
            .filter(([, value]) => value !== undefined && value !== null)
            .map(([key, value]) => [
                key,
                value instanceof Date
                    ? value.toISOString()
                    : String(value)
            ])
    );

    const response = await fetch(`${API_URL}/Results?${params}`);

    if (!response.ok) {
        throw new Error("Не удалось получить результаты");
    }

    return response.json();
}
export async function getValues(file: string): Promise<Value[]> {
    const params = new URLSearchParams();

    if (file) {
        params.set("file", file);
    }
    const response = await fetch(`${API_URL}/Values?${params}`, {})
    if (!response.ok) {
        throw new Error(response.statusText);
    }
    return response.json();
}
export async function valuesUpload(file:File) {
    const formData = new FormData();
    formData.append("file", file);
    const response = await fetch(`${API_URL}/Values/upload`, {method: "POST", body: formData});
    if (!response.ok) {
        throw new Error(response.statusText);
    }
    return response.text();
}