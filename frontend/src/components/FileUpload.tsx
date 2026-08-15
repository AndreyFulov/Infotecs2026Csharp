import { useState } from "react";
import { valuesUpload } from "../services/api";

interface FileUploadProps {
    onSuccess: (message: string) => void;
}

export function FileUpload({onSuccess}:FileUploadProps) {
    const [file, setFile] = useState<File | null>(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [result, setResult] = useState<string | null>(null);

    const handleUpload = async () => {
        if (!file) {
            return;
        }

        try {
            setLoading(true);
            setError(null);

            setResult(await valuesUpload(file));

            setFile(null);
        } catch (error) {
            setError(
                error instanceof Error
                    ? error.message
                    : "Неизвестная ошибка"
            );
        } finally {
            setLoading(false);
        }
        if(result != null) {
            onSuccess(result);
        }
    };

    return (
        <div>
            <input
                className="text-sm text-stone-500
   file:mr-5 file:py-1 file:px-3 file:border-[1px]
   file:text-xs file:font-medium
   file:bg-stone-50 file:text-stone-700
   hover:file:cursor-pointer hover:file:bg-blue-50
   hover:file:text-blue-700"
                type="file"
                accept=".csv"
                onChange={(event) => {
                    const selectedFile = event.target.files?.[0];

                    if (selectedFile) {
                        setFile(selectedFile);
                    }
                }}
            />

            <button
                onClick={handleUpload}
                disabled={!file || loading}
            >
                {loading ? "Загрузка..." : "Загрузить"}
            </button>

            {file && (
                <p>
                    Выбран файл: {file.name}
                </p>
            )}

            {error && (
                <p>
                    Ошибка: {error}
                </p>
            )}
        </div>
    );
}