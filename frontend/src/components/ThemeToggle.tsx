import { useEffect, useState } from "react";

export default function ThemeToggle() {
    // 1. Initialize state based on localStorage or fallback to system preference
    const [theme, setTheme] = useState(() => {
        if (typeof window !== "undefined") {
            return localStorage.getItem("theme") || "light";
        }
        return "light";
    });

    // 2. Watch for theme changes and add/remove the 'dark' class on the html root
    useEffect(() => {
        const root = window.document.documentElement;
        if (theme === "dark") {
            root.classList.add("dark");
            localStorage.setItem("theme", "dark");
        } else {
            root.classList.remove("dark");
            localStorage.setItem("theme", "light");
        }
    }, [theme]);

    // 3. Toggle helper function
    const toggleTheme = () => {
        setTheme((prev) => (prev === "light" ? "dark" : "light"));
    };

    return (
        <button
            onClick={toggleTheme}
            className="px-2 py-2 text-sm font-semibold transition-colors duration-200 border rounded-lg bg-slate-100 dark:bg-slate-800 text-slate-800 dark:text-slate-100 border-slate-300 dark:border-slate-600"
        >
            {theme === "light" ? "🌙" : "☀️"}
        </button>
    );
}
