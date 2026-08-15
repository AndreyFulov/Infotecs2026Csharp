import {FileUpload} from "./FileUpload.tsx";
import {useState} from "react";

export default function ValueUpload() {
    const [message, setMessage] = useState("");
    return (
        <main className="mx-auto max-w-7xl space-y-6 px-6 py-8">

            {/* Заголовок */}
            <div>
                <div className="flex">
                    <span className="m-2 dark:bg-yellow-400 dark:text-yellow-100 inline-flex items-center rounded-md bg-yellow-400/10 px-2 py-1 text-xs font-medium text-yellow-500 inset-ring inset-ring-yellow-400/20">POST</span>
                    <h1 className="text-3xl font-bold">
                        Значения
                    </h1>
                </div>

                <p className="mt-1 text-gray-500 dark:text-gray-200">
                    Загрузка CSV файлов и их обработка
                </p>
            </div>
            <details className="bg-green-400 dark:bg-gray-800 px-4 py-2 rounded-lg">
                <summary>
                    Сделать запрос
                </summary>
                <div className="rounded-lg border px-5 py-3 my-2">
                <FileUpload onSuccess={message=>setMessage(message)} />
                </div>
                {message && (
                    <div className="rounded-lg border border-red-200 bg-red-50 p-4 text-green-700">
                        {message}
                    </div>
                )}
            </details>
        </main>
    )
}