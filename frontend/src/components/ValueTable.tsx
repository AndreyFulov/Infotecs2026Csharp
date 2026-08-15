import { useState } from "react";
import type { Value } from "../models/Value";
import { getValues } from "../services/api.ts";
import FilterInput from "./FilterInput.tsx";

export function ValueTable() {
    const [file, setFile] = useState("");

    const [values, setValues] = useState<Value[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleSearch = async () => {
        try {
            setLoading(true);
            setError(null);

            const data = await getValues(file);

            setValues(data);
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
                    <span className="m-2 inline-flex items-center rounded-md bg-green-50 px-2 py-1 text-xs font-medium text-green-700 inset-ring inset-ring-green-600/20 dark:bg-green-400">
                        GET
                    </span>

                    <h1 className="text-3xl font-bold">
                        Значения
                    </h1>
                </div>

                <p className="mt-1 text-gray-500 dark:text-gray-200">
                    Получение значений из загруженного CSV-файла
                </p>
            </div>

            {/* Запрос */}
            <details
                open
                className="rounded-lg bg-green-400 px-4 py-2 dark:bg-gray-800"
            >
                <summary className="cursor-pointer">
                    Сделать запрос
                </summary>

                <div className="mt-2 rounded-lg bg-gray-100 px-4 py-5 dark:bg-green-900">

                    <section className="rounded-xl border bg-white p-6 shadow-sm dark:bg-gray-800">

                        <div className="max-w-md">
                            <FilterInput
                                label="Имя файла"
                                value={file}
                                onChange={setFile}
                                placeholder="test.csv"
                            />
                        </div>

                        <button
                            onClick={handleSearch}
                            disabled={loading || !file}
                            className="mt-6 rounded-lg bg-black px-5 py-2.5 text-sm font-medium text-white transition hover:bg-gray-800 disabled:cursor-not-allowed disabled:opacity-50"
                        >
                            {loading ? "Поиск..." : "Поиск"}
                        </button>

                    </section>

                    {/* Ошибка */}
                    {error && (
                        <div className="mt-4 rounded-lg border border-red-200 bg-red-50 p-4 text-red-700">
                            {error}
                        </div>
                    )}

                    {/* Результаты */}
                    <details className="mt-4">
                        <summary className="mb-5 cursor-pointer text-lg font-semibold">
                            Результаты поиска
                        </summary>

                        <section className="overflow-hidden rounded-xl border bg-white shadow-sm dark:bg-gray-800">

                            {loading ? (
                                <div className="p-8 text-center text-gray-500">
                                    Загрузка...
                                </div>
                            ) : values.length === 0 ? (
                                <div className="p-8 text-center text-gray-500">
                                    Значения не найдены
                                </div>
                            ) : (
                                <div className="overflow-x-auto">
                                    <table className="w-full text-left text-sm">

                                        <thead className="bg-green-100 dark:bg-gray-800">
                                        <tr>
                                            <th className="px-6 py-3">
                                                Дата
                                            </th>

                                            <th className="px-6 py-3">
                                                Время выполнения
                                            </th>

                                            <th className="px-6 py-3">
                                                Значение
                                            </th>

                                            <th className="px-6 py-3">
                                                Файл
                                            </th>
                                        </tr>
                                        </thead>

                                        <tbody className="divide-y dark:bg-gray-500">
                                        {values.map((value, index) => (
                                            <tr
                                                key={`${value.date}-${index}`}
                                                className="transition hover:bg-gray-50 dark:hover:bg-gray-600"
                                            >
                                                <td className="px-6 py-4">
                                                    {new Date(
                                                        value.date
                                                    ).toLocaleString()}
                                                </td>

                                                <td className="px-6 py-4">
                                                    {value.executionTime.toFixed(2)}
                                                    {" сек."}
                                                </td>

                                                <td className="px-6 py-4">
                                                    {value.value.toFixed(2)}
                                                </td>

                                                <td className="px-6 py-4 font-medium">
                                                    {value.resultFileName}
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