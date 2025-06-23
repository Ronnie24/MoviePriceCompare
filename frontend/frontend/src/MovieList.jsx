import { useState } from "react";
import "./MovieList.css";

function MovieList() {
  const [movies, setMovies] = useState([]);
  const [loading, setLoading] = useState(false);
  const [loaded, setLoaded] = useState(false);
  const [error, setError] = useState("");

  // Async function to fetch movie price comparison data from backend API
  const fetchData = async () => {
    setLoading(true);
    setLoaded(false);
    setError("");

    const timeout = new Promise((_, reject) =>
      setTimeout(
        () => reject(new Error("Request timed out. Please try again.")),
        180000
      )
    );

    const fetchRequest = fetch(
      "https://localhost:7236/api/Movie/compare-prices" //Replace the fetch URL with your actual backend URL if needed
    )
      .then((res) => {
        if (!res.ok) {
          throw new Error("Server responded with an error");
        }
        return res.json();
      })
      .then((data) => {
        setMovies(data);
        setLoaded(true);
      });

    try {
      await Promise.race([fetchRequest, timeout]);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container">
      <h1>🎬 Movie Price Comparison</h1>
      <p className="description">
        📽️ Compare movie ticket prices from <strong>Cinemaworld</strong> and{" "}
        <strong>Filmworld</strong>. This app helps you quickly find the cheapest
        deal for each movie in real time.
      </p>

      {!loaded && !loading && !error && (
        <button onClick={fetchData} className="fetch-button">
          Get Lowest Prices
        </button>
      )}

      {loading && (
        <div className="loader-container">
          <div className="loader"></div>
          <p className="loading">Fetching movie data, please wait...</p>
        </div>
      )}

      {error && (
        <div className="error-container">
          <p className="error">{error}</p>
          <button onClick={fetchData} className="refresh-button">
            🔁 Try Again
          </button>
        </div>
      )}

      {loaded && !error && (
        <>
          <table>
            <thead>
              <tr>
                <th>Title</th>
                <th>Lowest Price</th>
                <th>Provider</th>
              </tr>
            </thead>
            <tbody>
              {movies.map((movie, idx) => (
                <tr key={idx}>
                  <td>{movie.title}</td>
                  <td>${movie.lowestPrice.toFixed(2)}</td>
                  <td>{movie.provider}</td>
                </tr>
              ))}
            </tbody>
          </table>
          <button onClick={fetchData} className="refresh-button">
            🔄 Refresh
          </button>
        </>
      )}
    </div>
  );
}

export default MovieList;
