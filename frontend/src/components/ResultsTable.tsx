import { useState } from "react";
import type { Result } from "../models/Result";
import type { ResultQuery } from "../models/ResultQuery";
import {getResults} from "../services/api.ts";
import FilterInput from "./FilterInput.tsx";

export function ResultsTable() {
    const [name, setName] = useState("");
    const [dateFrom, setDateFrom] = useState("");
    const [dateTo, setDateTo] = useState("");
    const [valueFrom, setValueFrom] = useState("");
    const [valueTo, setValueTo] = useState("");
    const [executionTimeFrom, setExecutionTimeFrom] = useState("");
    const [executionTimeTo, setExecutionTimeTo] = useState("");

    const [results, setResults] = useState<Result[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleSearch = async () => {
        try {
            setLoading(true);
            setError(null);

            const query: ResultQuery = {
                ...(name && { name }),
                ...(dateFrom && { dateFrom: new Date(dateFrom) }),
                ...(dateTo && { dateTo: new Date(dateTo) }),
                ...(valueFrom && { valueFrom: Number(valueFrom) }),
                ...(valueTo && { valueTo: Number(valueTo) }),
                ...(executionTimeFrom && {
                    executionTimeFrom: Number(executionTimeFrom)
                }),
                ...(executionTimeTo && {
                    executionTimeTo: Number(executionTimeTo)
                })
            };

            const data = await getResults(query);

            setResults(data);
        } catch (e) {
            setError(
                e instanceof Error
                    ? e.message
                    : "Произошла неизвестная ошибка"
            );
        } finally {
            setLoading(false);
        }
    };

    return (
        <main className="mx-auto max-w-7xl space-y-6 px-6 py-8">
            
            {/* Заголовок */}
            <div>
                <div className="flex">
                <span className="m-2 inline-flex items-center rounded-md dark:bg-green-400 bg-green-50 px-2 py-1 text-xs font-medium text-green-700 inset-ring inset-ring-green-600/20">GET</span>
                <h1 className="text-3xl font-bold">
                    Результаты
                </h1>
                </div>

                <p className="mt-1 text-gray-500 dark:text-gray-200">
                    Поиск и анализ загруженных CSV-файлов
                </p>
            </div>
            <details className="bg-green-400 dark:bg-gray-800 px-4 py-2 rounded-lg">
                <summary>
                    Сделать запрос
                </summary>
                {/* Фильтры */}
                <div className="px-4 py-2 bg-gray-100 dark:bg-green-900 rounded-lg">
                    <details>
                        <summary className="mb-5 text-lg font-semibold">
                            Фильтры
                        </summary>
                        <section className="rounded-xl border bg-white dark:bg-gray-800 p-6 shadow-sm">

                            <div className="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-4">

                                <FilterInput
                                    label="Имя файла"
                                    value={name}
                                    onChange={setName}
                                    placeholder="test"
                                />

                                <FilterInput
                                    label="Дата от"
                                    type="date"
                                    value={dateFrom}
                                    onChange={setDateFrom}
                                />

                                <FilterInput
                                    label="Дата до"
                                    type="date"
                                    value={dateTo}
                                    onChange={setDateTo}
                                />

                                <FilterInput
                                    label="Значение от"
                                    type="number"
                                    value={valueFrom}
                                    onChange={setValueFrom}
                                />

                                <FilterInput
                                    label="Значение до"
                                    type="number"
                                    value={valueTo}
                                    onChange={setValueTo}
                                />

                                <FilterInput
                                    label="Время выполнения от"
                                    type="number"
                                    value={executionTimeFrom}
                                    onChange={setExecutionTimeFrom}
                                />

                                <FilterInput
                                    label="Время выполнения до"
                                    type="number"
                                    value={executionTimeTo}
                                    onChange={setExecutionTimeTo}
                                />

                            </div>


                        </section>
                    </details>
                    <button
                        onClick={handleSearch}
                        disabled={loading}
                        className="mt-6 rounded-lg bg-black px-5 py-2.5 text-sm font-medium text-white transition hover:bg-gray-800 disabled:cursor-not-allowed disabled:opacity-50"
                    >
                        {loading ? "Поиск..." : "Поиск"}
                    </button>
                    {/* Ошибка */}
                    {error && (
                        <div className="rounded-lg border border-red-200 bg-red-50 p-4 text-red-700">
                            {error}
                        </div>
                    )}

                    {/* Результаты */}
                    <details>
                        <summary className="mb-5 text-lg font-semibold">
                            Результаты поиска
                        </summary>
                        <section className="overflow-hidden rounded-xl border bg-white shadow-sm">

                            {loading ? (
                                <div className="p-8 text-center text-gray-500">
                                    Загрузка...
                                </div>
                            ) : results.length === 0 ? (
                                <div className="p-8 text-center text-gray-500">
                                    Результаты не найдены
                                </div>
                            ) : (
                                <div className="overflow-x-auto">
                                    <table className="w-full text-left text-sm">

                                        <thead className="bg-green-100 dark:bg-gray-800">
                                        <tr>
                                            <th className="px-6 py-3">
                                                Файл
                                            </th>

                                            <th className="px-6 py-3">
                                                Запущен
                                            </th>

                                            <th className="px-6 py-3">
                                                Длительность
                                            </th>

                                            <th className="px-6 py-3">
                                                Среднее значение
                                            </th>

                                            <th className="px-6 py-3">
                                                Среднее время
                                            </th>

                                            <th className="px-6 py-3">
                                                Медиана
                                            </th>

                                            <th className="px-6 py-3">
                                                Минимум
                                            </th>

                                            <th className="px-6 py-3">
                                                Максимум
                                            </th>
                                        </tr>
                                        </thead>

                                        <tbody className="divide-y dark:bg-gray-500">
                                        {results.map((result) => (
                                            <tr
                                                key={result.name}
                                                className="transition hover:bg-gray-50"
                                            >
                                                <td className="px-6 py-4 font-medium">
                                                    {result.name}
                                                </td>

                                                <td className="px-6 py-4">
                                                    {new Date(
                                                        result.startedAt
                                                    ).toLocaleString()}
                                                </td>

                                                <td className="px-6 py-4">
                                                    {result.durationSeconds} сек.
                                                </td>

                                                <td className="px-6 py-4">
                                                    {result.averageValue.toFixed(2)}
                                                </td>

                                                <td className="px-6 py-4">
                                                    {result.averageExecutionTime.toFixed(2)} сек.
                                                </td>

                                                <td className="px-6 py-4">
                                                    {result.medianValue.toFixed(2)}
                                                </td>

                                                <td className="px-6 py-4">
                                                    {result.minValue.toFixed(2)}
                                                </td>

                                                <td className="px-6 py-4">
                                                    {result.maxValue.toFixed(2)}
                                                </td>
                                            </tr>
                                        ))}
                                        </tbody>

                                    </table>
                                </div>
                            )}
                        </section>
                    </details>
                </div>
            </details>
        </main>
    );
}