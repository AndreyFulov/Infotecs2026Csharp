import ThemeToggle from "./ThemeToggle.tsx";

export default function Header() {
    return (
        <header className="bg-green-500 dark:bg-green-900 text-white shadow-2xl">
            <div className="mx-auto flex h-16 max-w-7xl items-center justify-between px-6">

                <div className="flex items-center gap-8">
                    <a
                        href="/"
                        className="text-xl font-semibold tracking-tight"
                    >
                        Egoshin to Infotecs
                    </a>
                </div>

                <ThemeToggle/>
            </div>
        </header>
    );
}