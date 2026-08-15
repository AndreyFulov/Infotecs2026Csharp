import Header from "./components/Header.tsx";
import {ResultsTable} from "./components/ResultsTable.tsx";
import ValueUpload from "./components/ValueUpload.tsx";
import {ValueTable} from "./components/ValueTable.tsx";

export default function App() {
    return (
        <div className="min-h-screen bg-gray-50 dark:bg-gray-800">
            <Header />

            <main className="mx-auto max-w-7xl px-6 py-8 dark:text-white mt-10">
                <div className="bg-green-100 dark:bg-green-700 rounded-2xl my-5">
                    <ResultsTable/>
                </div>
                <div className="bg-green-100 dark:bg-green-700 rounded-2xl my-5">
                    <ValueUpload/>
                </div>
                <div className="bg-green-100 dark:bg-green-700 rounded-2xl my-5">
                    <ValueTable/>
                </div>
            </main>
        </div>
    );
}