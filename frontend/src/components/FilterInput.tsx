interface FilterInputProps {
    label: string;
    value: string;
    onChange: (value: string) => void;
    type?: string;
    placeholder?: string;
}

function FilterInput({
                         label,
                         value,
                         onChange,
                         type = "text",
                         placeholder
                     }: FilterInputProps) {
    return (
        <div>
            <label className="mb-1.5 block text-sm font-medium">
                {label}
            </label>

            <input
                type={type}
                value={value}
                placeholder={placeholder}
                onChange={(e) => onChange(e.target.value)}
                className="w-full rounded-lg border px-3 py-2 outline-none transition focus:border-black"
            />
        </div>
    );
}
export default FilterInput;