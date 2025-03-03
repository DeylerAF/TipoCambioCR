"use client";

import { useState } from "react";

interface ExchangeRate {
    fecha: string;
    tipoCambioCompra: number;
    tipoCambioVenta: number;
}

export default function Home() {
    const [data, setData] = useState<ExchangeRate[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    // Función para obtener la fecha actual formateada a "yyyy-MM-dd"
    const formatISODate = (date: Date) => date.toISOString().split("T")[0];

    // Función para obtener las fechas iniciales
    const getInitialDates = () => {
        const now = new Date();
        const lastMonth = new Date();
        lastMonth.setMonth(now.getMonth() - 1);
        return {
            startDate: formatISODate(lastMonth),
            endDate: formatISODate(now),
        };
    };

    // Obtener fechas iniciales
    const { startDate: initialStartDate, endDate: initialEndDate } =
        getInitialDates();

    const [startDate, setStartDate] = useState(initialStartDate);
    const [endDate, setEndDate] = useState(initialEndDate);

    // Función para formatear la fecha de "yyyy-MM-dd" a "dd/MM/yyyy"
    const formatToDisplay = (dateStr: string) =>
        new Date(dateStr + "T00:00:00").toLocaleDateString("en-GB");

    const formattedStartDate = formatToDisplay(startDate);
    const formattedEndDate = formatToDisplay(endDate);

    const fetchExchangeRates = async () => {
        setLoading(true);
        setError(null);
        setData([]);

        try {
            const res = await fetch(
                `http://localhost:5099/api/ExchangeRate?startDate=${formattedStartDate}&endDate=${formattedEndDate}`
            );
            const result = await res.json();

            console.log("API Response:", result);

            if (Array.isArray(result)) {
                setData(result);
            } else {
                setError("Formato de datos incorrecto.");
            }
        } catch (err) {
            setError("Error al obtener los datos.");
        } finally {
            setLoading(false);
        }
    };

    const fetchExchangeRateToday = async () => {
        setLoading(true);
        setError(null);
        setData([]);

        try {
            const todayISO = formatISODate(new Date());
            const formattedToday = formatToDisplay(todayISO);
            const res = await fetch(
                `http://localhost:5099/api/ExchangeRate?startDate=${formattedToday}&endDate=${formattedToday}`
            );
            const result = await res.json();

            console.log("API Response (hoy):", result);

            if (Array.isArray(result) && result.length > 0) {
                setData(result);
            } else {
                setError("Formato de datos incorrecto.");
            }
        } catch (err) {
            setError("Error al obtener los datos.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex flex-col items-center justify-center bg-zinc-100 p-4">
            <div className="bg-white shadow-md rounded-lg p-6 w-full max-w-lg">
                <h1 className="text-2xl font-bold text-center text-zinc-800">
                    Tipo de Cambio
                </h1>

                {/* Selección de Fechas */}
                <div className="flex flex-col md:flex-row gap-4 mt-4">
                    <div className="flex flex-col">
                        <label className="text-sm font-medium text-zinc-600">
                            Fecha Inicio
                        </label>
                        <input
                            type="date"
                            value={startDate}
                            onChange={(e) => setStartDate(e.target.value)}
                            className="border p-2 rounded-md text-zinc-600"
                        />
                    </div>
                    <div className="flex flex-col">
                        <label className="text-sm font-medium text-zinc-600">
                            Fecha Fin
                        </label>
                        <input
                            type="date"
                            value={endDate}
                            onChange={(e) => setEndDate(e.target.value)}
                            className="border p-2 rounded-md text-zinc-600"
                        />
                    </div>
                </div>

                <button
                    onClick={fetchExchangeRates}
                    className="mt-4 w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition"
                >
                    Consultar por rango de fechas
                </button>

                <button
                    onClick={fetchExchangeRateToday}
                    className="mt-4 w-full bg-green-600 text-white py-2 rounded-md hover:bg-green-700 transition"
                >
                    Obtener tipo de cambio de hoy
                </button>

                {loading && (
                    <p className="text-center mt-4 text-zinc-600">
                        Cargando...
                    </p>
                )}
                {error && (
                    <p className="text-red-500 text-center mt-4 text-zinc-600">
                        {error}
                    </p>
                )}

                {/* Mostrar los datos en una tabla */}
                {!loading && !error && data.length > 0 && (
                    <table className="w-full mt-4 border-collapse border border-zinc-300">
                        <thead>
                            <tr className="bg-zinc-200">
                                <th className="border p-2 text-zinc-600">
                                    Fecha
                                </th>
                                <th className="border p-2 text-zinc-600">
                                    Compra
                                </th>
                                <th className="border p-2 text-zinc-600">
                                    Venta
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            {data.map((item, index) => (
                                <tr
                                    key={index}
                                    className="text-center text-zinc-600"
                                >
                                    <td className="border p-2">{item.fecha}</td>
                                    <td className="border p-2 text-green-600">
                                        {item.tipoCambioCompra
                                            ? item.tipoCambioCompra.toFixed(2)
                                            : "N/A"}
                                    </td>
                                    <td className="border p-2 text-red-600">
                                        {item.tipoCambioVenta
                                            ? item.tipoCambioVenta.toFixed(2)
                                            : "N/A"}
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </div>
        </div>
    );
}