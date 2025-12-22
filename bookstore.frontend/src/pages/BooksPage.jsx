import { useMemo, useState } from "react";

// dummy data 
const dummyBooks = [
    {
        id: 1,
        title: "Clean Code",
        author: "Robert C. Martin",
        category: "Software",
        price: 250,
        publishYear: 2008,
        coverUrl:
            "https://images.pexels.com/photos/590493/pexels-photo-590493.jpeg?auto=compress&w=400",
    },
    {
        id: 2,
        title: "The Pragmatic Programmer",
        author: "Andrew Hunt",
        category: "Software",
        price: 280,
        publishYear: 1999,
        coverUrl:
            "https://images.pexels.com/photos/46274/pexels-photo-46274.jpeg?auto=compress&w=400",
    },
    {
        id: 3,
        title: "1984",
        author: "George Orwell",
        category: "Fiction",
        price: 90,
        publishYear: 1949,
        coverUrl:
            "https://images.pexels.com/photos/46274/pexels-photo-46274.jpeg?auto=compress&w=400",
    },
    {
        id: 4,
        title: "Sapiens",
        author: "Yuval Noah Harari",
        category: "History",
        price: 150,
        publishYear: 2011,
        coverUrl:
            "https://images.pexels.com/photos/46274/pexels-photo-46274.jpeg?auto=compress&w=400",
    },
];

function BooksPage() {
    // filters
    const [query, setQuery] = useState("");
    const [category, setCategory] = useState("all");
    const [minPrice, setMinPrice] = useState("");
    const [maxPrice, setMaxPrice] = useState("");
    const [minYear, setMinYear] = useState("");
    const [maxYear, setMaxYear] = useState("");
    const [sortBy, setSortBy] = useState("title-asc");

    const categories = useMemo(() => {
        const set = new Set(dummyBooks.map((b) => b.category));
        return ["all", ...Array.from(set)];
    }, []);

    const filteredBooks = useMemo(() => {
        return dummyBooks
            .filter((book) => {
                const q = query.toLowerCase().trim();

                if (q) {
                    const text =
                        (book.title + " " + book.author + " " + book.category).toLowerCase();
                    if (!text.includes(q)) return false;
                }

                if (category !== "all" && book.category !== category) return false;

                if (minPrice !== "" && book.price < Number(minPrice)) return false;
                if (maxPrice !== "" && book.price > Number(maxPrice)) return false;

                if (minYear !== "" && book.publishYear < Number(minYear)) return false;
                if (maxYear !== "" && book.publishYear > Number(maxYear)) return false;

                return true;
            })
            .sort((a, b) => {
                switch (sortBy) {
                    case "price-asc":
                        return a.price - b.price;
                    case "price-desc":
                        return b.price - a.price;
                    case "year-asc":
                        return a.publishYear - b.publishYear;
                    case "year-desc":
                        return b.publishYear - a.publishYear;
                    case "title-desc":
                        return b.title.localeCompare(a.title);
                    case "title-asc":
                    default:
                        return a.title.localeCompare(b.title);
                }
            });
    }, [query, category, minPrice, maxPrice, minYear, maxYear, sortBy]);

    const handleClearFilters = () => {
        setQuery("");
        setCategory("all");
        setMinPrice("");
        setMaxPrice("");
        setMinYear("");
        setMaxYear("");
        setSortBy("title-asc");
    };

    return (
        <div
            style={{
                minHeight: "100vh",
                fontFamily: "system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI'",
                backgroundColor: "#f3f4f6", // light gray background
                color: "#111827",
                display: "flex",
                flexDirection: "column",
            }}
        >
            {/* NAVBAR */}
            <header
                style={{
                    position: "sticky",
                    top: 0,
                    zIndex: 10,
                    backgroundColor: "#ffffff",
                    borderBottom: "1px solid #e5e7eb",
                }}
            >
                <div
                    style={{
                        maxWidth: "1200px",
                        margin: "0 auto",
                        padding: "0.75rem 1rem",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "space-between",
                        gap: "1.5rem",
                    }}
                >
                    {/* logo + title */}
                    <div
                        style={{
                            display: "flex",
                            alignItems: "center",
                            gap: "0.75rem",
                            minWidth: 0,
                        }}
                    >
                        <span style={{ fontSize: "1.6rem" }}>📚</span>
                        <div style={{ display: "flex", flexDirection: "column" }}>
                            <span style={{ fontWeight: 700, fontSize: "1.05rem" }}>
                                BookStore
                            </span>
                            <span
                                style={{
                                    fontSize: "0.8rem",
                                    color: "#6b7280",
                                    whiteSpace: "nowrap",
                                }}
                            >
                                Browse, filter and manage books
                            </span>
                        </div>
                    </div>

                    {/* menu */}
                    <nav
                        style={{
                            display: "flex",
                            gap: "1rem",
                            fontSize: "0.9rem",
                            flexWrap: "wrap",
                            justifyContent: "flex-end",
                        }}
                    >
                        {["Home", "Categories", "Authors", "User Account"].map((item, idx) => (
                            <button
                                key={item}
                                style={{
                                    backgroundColor: idx === 0 ? "#111827" : "transparent",
                                    color: idx === 0 ? "#f9fafb" : "#374151",
                                    borderRadius: "999px",
                                    border:
                                        idx === 0 ? "1px solid #111827" : "1px solid transparent",
                                    padding: "0.3rem 0.9rem",
                                    cursor: "pointer",
                                    fontSize: "0.85rem",
                                    whiteSpace: "nowrap",
                                }}
                            >
                                {item}
                            </button>
                        ))}
                    </nav>
                </div>
            </header>

            {/* MAIN CONTENT */}
            <main
                style={{
                    flex: 1,
                    maxWidth: "1200px",
                    margin: "0 auto",
                    padding: "1.5rem 1rem 2rem",
                    width: "100%",
                    boxSizing: "border-box",
                }}
            >
                {/* FILTER CARD */}
                <section
                    style={{
                        backgroundColor: "#ffffff",
                        borderRadius: "0.9rem",
                        padding: "1rem 1.1rem",
                        border: "1px solid #e5e7eb",
                        marginBottom: "1.5rem",
                        boxShadow: "0 4px 10px rgba(15,23,42,0.03)",
                    }}
                >
                    <div
                        style={{
                            display: "flex",
                            justifyContent: "space-between",
                            alignItems: "flex-start",
                            gap: "0.75rem",
                            flexWrap: "wrap",
                            marginBottom: "0.75rem",
                        }}
                    >
                        <div>
                            <h2
                                style={{
                                    margin: 0,
                                    fontSize: "1rem",
                                    fontWeight: 600,
                                }}
                            >
                                Search & Filters
                            </h2>
                            <p
                                style={{
                                    margin: 0,
                                    marginTop: "0.2rem",
                                    fontSize: "0.8rem",
                                    color: "#6b7280",
                                }}
                            >
                                Filter by title, author, category, price range and publish year.
                            </p>
                        </div>

                        <button
                            type="button"
                            onClick={handleClearFilters}
                            style={{
                                borderRadius: "999px",
                                border: "1px solid #d1d5db",
                                backgroundColor: "#f9fafb",
                                fontSize: "0.8rem",
                                padding: "0.25rem 0.8rem",
                                cursor: "pointer",
                                color: "#374151",
                                whiteSpace: "nowrap",
                            }}
                        >
                            Clear filters
                        </button>
                    </div>

                    <div
                        style={{
                            display: "grid",
                            gridTemplateColumns: "minmax(0, 2fr) minmax(0, 1.3fr)",
                            gap: "0.75rem",
                        }}
                    >
                        {/* Search input */}
                        <div style={{ display: "flex", flexDirection: "column", gap: "0.3rem" }}>
                            <label style={{ fontSize: "0.8rem", color: "#6b7280" }}>
                                Search (title, author, category)
                            </label>
                            <input
                                type="text"
                                value={query}
                                onChange={(e) => setQuery(e.target.value)}
                                placeholder="e.g. Clean Code, Orwell, Software…"
                                style={{
                                    padding: "0.45rem 0.7rem",
                                    borderRadius: "0.5rem",
                                    border: "1px solid #d1d5db",
                                    fontSize: "0.9rem",
                                    outline: "none",
                                }}
                            />
                        </div>

                        {/* Category + sort */}
                        <div
                            style={{
                                display: "grid",
                                gridTemplateColumns: "minmax(0, 1fr) minmax(0, 1fr)",
                                gap: "0.75rem",
                            }}
                        >
                            <div style={{ display: "flex", flexDirection: "column", gap: "0.3rem" }}>
                                <label style={{ fontSize: "0.8rem", color: "#6b7280" }}>
                                    Category
                                </label>
                                <select
                                    value={category}
                                    onChange={(e) => setCategory(e.target.value)}
                                    style={{
                                        padding: "0.45rem 0.7rem",
                                        borderRadius: "0.5rem",
                                        border: "1px solid #d1d5db",
                                        fontSize: "0.9rem",
                                        backgroundColor: "#ffffff",
                                    }}
                                >
                                    {categories.map((cat) => (
                                        <option key={cat} value={cat}>
                                            {cat === "all" ? "All categories" : cat}
                                        </option>
                                    ))}
                                </select>
                            </div>

                            <div style={{ display: "flex", flexDirection: "column", gap: "0.3rem" }}>
                                <label style={{ fontSize: "0.8rem", color: "#6b7280" }}>
                                    Sort by
                                </label>
                                <select
                                    value={sortBy}
                                    onChange={(e) => setSortBy(e.target.value)}
                                    style={{
                                        padding: "0.45rem 0.7rem",
                                        borderRadius: "0.5rem",
                                        border: "1px solid #d1d5db",
                                        fontSize: "0.9rem",
                                        backgroundColor: "#ffffff",
                                    }}
                                >
                                    <option value="title-asc">Title (A → Z)</option>
                                    <option value="title-desc">Title (Z → A)</option>
                                    <option value="price-asc">Price (low → high)</option>
                                    <option value="price-desc">Price (high → low)</option>
                                    <option value="year-asc">Publish year (old → new)</option>
                                    <option value="year-desc">Publish year (new → old)</option>
                                </select>
                            </div>
                        </div>

                        {/* Price range */}
                        <div
                            style={{
                                display: "grid",
                                gridTemplateColumns: "1fr 1fr",
                                gap: "0.75rem",
                            }}
                        >
                            <div style={{ display: "flex", flexDirection: "column", gap: "0.3rem" }}>
                                <label style={{ fontSize: "0.8rem", color: "#6b7280" }}>
                                    Min price (₺)
                                </label>
                                <input
                                    type="number"
                                    value={minPrice}
                                    onChange={(e) => setMinPrice(e.target.value)}
                                    placeholder="min"
                                    style={{
                                        padding: "0.45rem 0.7rem",
                                        borderRadius: "0.5rem",
                                        border: "1px solid #d1d5db",
                                        fontSize: "0.9rem",
                                    }}
                                />
                            </div>

                            <div style={{ display: "flex", flexDirection: "column", gap: "0.3rem" }}>
                                <label style={{ fontSize: "0.8rem", color: "#6b7280" }}>
                                    Max price (₺)
                                </label>
                                <input
                                    type="number"
                                    value={maxPrice}
                                    onChange={(e) => setMaxPrice(e.target.value)}
                                    placeholder="max"
                                    style={{
                                        padding: "0.45rem 0.7rem",
                                        borderRadius: "0.5rem",
                                        border: "1px solid #d1d5db",
                                        fontSize: "0.9rem",
                                    }}
                                />
                            </div>
                        </div>

                        {/* Publish year range */}
                        <div
                            style={{
                                display: "grid",
                                gridTemplateColumns: "1fr 1fr",
                                gap: "0.75rem",
                            }}
                        >
                            <div style={{ display: "flex", flexDirection: "column", gap: "0.3rem" }}>
                                <label style={{ fontSize: "0.8rem", color: "#6b7280" }}>
                                    Min publish year
                                </label>
                                <input
                                    type="number"
                                    value={minYear}
                                    onChange={(e) => setMinYear(e.target.value)}
                                    placeholder="e.g. 2000"
                                    style={{
                                        padding: "0.45rem 0.7rem",
                                        borderRadius: "0.5rem",
                                        border: "1px solid #d1d5db",
                                        fontSize: "0.9rem",
                                    }}
                                />
                            </div>

                            <div style={{ display: "flex", flexDirection: "column", gap: "0.3rem" }}>
                                <label style={{ fontSize: "0.8rem", color: "#6b7280" }}>
                                    Max publish year
                                </label>
                                <input
                                    type="number"
                                    value={maxYear}
                                    onChange={(e) => setMaxYear(e.target.value)}
                                    placeholder="e.g. 2024"
                                    style={{
                                        padding: "0.45rem 0.7rem",
                                        borderRadius: "0.5rem",
                                        border: "1px solid #d1d5db",
                                        fontSize: "0.9rem",
                                    }}
                                />
                            </div>
                        </div>
                    </div>
                </section>

                {/* BOOK LIST */}
                <section>
                    <div
                        style={{
                            display: "flex",
                            justifyContent: "space-between",
                            gap: "0.75rem",
                            alignItems: "baseline",
                            flexWrap: "wrap",
                        }}
                    >
                        <h2
                            style={{
                                margin: 0,
                                fontSize: "1rem",
                                fontWeight: 600,
                            }}
                        >
                            Books ({filteredBooks.length})
                        </h2>
                        <p
                            style={{
                                margin: 0,
                                fontSize: "0.8rem",
                                color: "#6b7280",
                            }}
                        >
                            Showing results based on current filters.
                        </p>
                    </div>

                    {filteredBooks.length === 0 ? (
                        <p
                            style={{
                                marginTop: "0.75rem",
                                fontSize: "0.9rem",
                                color: "#6b7280",
                            }}
                        >
                            No books match the current filters.
                        </p>
                    ) : (
                        <div
                            style={{
                                marginTop: "0.75rem",
                                display: "grid",
                                gridTemplateColumns: "repeat(auto-fit, minmax(220px, 1fr))",
                                gap: "1rem",
                            }}
                        >
                            {filteredBooks.map((book) => (
                                <article
                                    key={book.id}
                                    style={{
                                        backgroundColor: "#ffffff",
                                        borderRadius: "0.75rem",
                                        padding: "0.9rem",
                                        border: "1px solid #e5e7eb",
                                        boxShadow: "0 4px 10px rgba(15,23,42,0.03)",
                                        display: "flex",
                                        flexDirection: "column",
                                        gap: "0.5rem",
                                    }}
                                >
                                    {/* cover image */}
                                    {book.coverUrl && (
                                        <div
                                            style={{
                                                width: "100%",
                                                borderRadius: "0.5rem",
                                                overflow: "hidden",
                                                backgroundColor: "#f3f4f6",
                                            }}
                                        >
                                            <img
                                                src={book.coverUrl}
                                                alt={book.title}
                                                style={{
                                                    width: "100%",
                                                    height: "210px",
                                                    objectFit: "cover",
                                                    display: "block",
                                                }}
                                            />
                                        </div>
                                    )}

                                    {/* title + category */}
                                    <div
                                        style={{
                                            display: "flex",
                                            justifyContent: "space-between",
                                            gap: "0.5rem",
                                            marginTop: "0.2rem",
                                        }}
                                    >
                                        <h3
                                            style={{
                                                margin: 0,
                                                fontSize: "1rem",
                                                fontWeight: 600,
                                            }}
                                        >
                                            {book.title}
                                        </h3>
                                        <span
                                            style={{
                                                fontSize: "0.7rem",
                                                borderRadius: "999px",
                                                padding: "0.15rem 0.55rem",
                                                backgroundColor: "#eff6ff",
                                                color: "#1d4ed8",
                                                whiteSpace: "nowrap",
                                            }}
                                        >
                                            {book.category}
                                        </span>
                                    </div>

                                    {/* author */}
                                    <p
                                        style={{
                                            margin: 0,
                                            fontSize: "0.9rem",
                                            color: "#6b7280",
                                        }}
                                    >
                                        {book.author}
                                    </p>

                                    {/* price + year */}
                                    <div
                                        style={{
                                            display: "flex",
                                            justifyContent: "space-between",
                                            alignItems: "center",
                                            marginTop: "0.3rem",
                                            fontSize: "0.9rem",
                                        }}
                                    >
                                        <span
                                            style={{
                                                fontWeight: 700,
                                                color: "#111827",
                                            }}
                                        >
                                            {book.price} ₺
                                        </span>
                                        <span
                                            style={{
                                                fontSize: "0.8rem",
                                                color: "#6b7280",
                                            }}
                                        >
                                            {book.publishYear}
                                        </span>
                                    </div>
                                </article>
                            ))}
                        </div>
                    )}
                </section>
            </main>
        </div>
    );
}

export default BooksPage;
