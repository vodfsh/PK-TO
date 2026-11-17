# Laboratorium 3 – Symulacja epidemii (Stan + Pamiątka)

**Technologie:** Blazor WebAssembly, C#. Renderowanie na `<canvas>`. Całość działa po stronie klienta.

## 1. Zadanie

Symulacja poruszania się osobników w pomieszczeniu `n x m` m. Na granicy osobnik zawraca lub opuszcza obszar (50 % /
50 %), nowi osobnicy wkraczają w losowych punktach granicy (10 % z nich jest zakażonych), a zakażenie przenosi się przez
kontakt. Stan symulacji ma być możliwy do zapisu i wczytania w dowolnym momencie.

## 2. Model symulacji (`Simulation.Advance()`)

Jedna sekunda symulacji = **25 kroków** (`Dt = 0,04 s`). Każdy krok przebiega w kolejności:

1. **Ruch**: z prawdopodobieństwem 2 % na osobnika prędkość jest obracana o maks. ±20°, a jej wartość losowana ponownie
   z zakresu 0,25–2,0 m/s (poniżej limitu 2,5 m/s). Następnie `Pos += Vel · Dt`.
2. **Granica**: osobnik poza pomieszczeniem jest usuwany z prawdopodobieństwem 50 %, w przeciwnym razie odbijany
   (pozycja przycięta, składowa prędkości odwrócona).
3. **Wkraczanie**: co 10 kroków, jeśli `People.Count < Max`, osobnik wchodzi w losowym punkcie losowego boku, skierowany
   do wnętrza. 10 % jest zakażonych; w pozostałych przypadkach ustawienie *% odpornych* decyduje, czy osobnik jest
   odporny.
4. **Kontakty**: dla każdego zdrowego osobnika `Contact` to najwyższe prawdopodobieństwo zakażenia wśród zakażonych w
   promieniu 2 m (0,5 bezobjawowy, 1,0 objawowy) lub 0.
5. **Aktualizacja stanu**: obiekt bieżącego stanu każdego osobnika aktualizuje tego osobnika.

Używany jest jeden generator z ziarnem, wywoływany w stałej kolejności,
więc przebieg jest w deterministyczny.

## 3. Wektory (Laboratorium 2)

`Vector2D` implementuje `IVector` (pozycja, `+`, `−`, mnożenie przez skalar, `DistanceSquared`). `V2DPolarInheritance`
go dziedziczy i implementuje `IPolar2D` (`Angle`, `Radius`, `Rotate`), dzięki czemu prędkość można obracać lub skalować
bez ręcznego przeliczania składowych. Pozycja to `Vector2D`, prędkość to `V2DPolarInheritance`.

## 4. Wzorzec Stan

`Person` przechowuje `IPersonState` i deleguje do niego `Update()`. Cztery stany to bezstanowe singletony (przez
`States.For(kind)`); zmienne dane (`Timer`, `Exposure`) są w `Person`.

| Stan                                                       | Zachowanie w `Update`                                                  | Przejście                                                                           |
|------------------------------------------------------------|------------------------------------------------------------------------|-------------------------------------------------------------------------------------|
| `HealthyState`                                             | Gdy `Contact > 0`, `Exposure += Dt`; w przeciwnym razie `Exposure = 0` | Po 3 s kontaktu losowanie z szansą `Contact`; sukces → `Infect()` (50 % objawowych) |
| `AsymptomaticState` / `SymptomaticState` (`InfectedState`) | `Timer -= Dt` (20–30 s losowane przy zakażeniu)                        | `Timer ≤ 0` → `Immune`                                                              |
| `ImmuneState`                                              | brak działań                                                           | brak                                                                                |

Zgodnie z treścią zadania zakażenie wymaga odległości ≤ 2 m **oraz** czasu ≥ 3 s, z prawdopodobieństwem 50 % / 100 %, a
zakażenie trwa 20–30 s, po czym daje odporność.

## 5. Wzorzec Pamiątka

- **Originatory:** `Person` → `PersonMemento` (id, pozycja, prędkość, rodzaj stanu, `Timer`, `Exposure`) oraz
  `Simulation` → `SimulationMemento`.
- **Pamiątka złożona:** `SimulationMemento` zawiera ustawienia, licznik kroków, stan RNG, licznik wkraczania, następny
  id oraz `List<PersonMemento>` – **pamiątki podrzędne**, po jednej na osobnika.
- **Odtwarzanie:** `Simulation.FromMemento()` odbudowuje każdy `Person` z jego pamiątki podrzędnej, więc przebieg jest
  kontynuowany identycznie, także losowa przyszłość, bo zapisany jest stan RNG.
- **CareTaker:** przechowuje listę migawek (`Save`, `Restore`, `Clear`) i nigdy nie zagląda do ich wnętrza. Pamiątki to
  niemutowalne rekordy (`record`).
- **Trwałość:** `SimulationJson` zapisuje pamiątkę przez `System.Text.Json` do pliku `.json` (*Download snapshot*) i
  wczytuje ją z powrotem (*Load snapshot*). `Contact` nie jest zapisywany, bo jest liczony od nowa w każdym kroku.

## 6. Interfejs i ustawienia

`Home.razor` ma trzy fazy: **Setup -> Running <-> Paused**. W fazie Setup można zmieniać ustawienia: rozmiar
pomieszczenia (domyślnie 20 × 20 m), populacja początkowa (100), populacja maksymalna (150, twardy limit wkraczania),
początkowa liczba zakażonych (5), % odpornych oraz ziarno (puste = losowe). 

## 7. Pliki

Folder `Sim/` zawiera logikę (`Simulation`, `Person`, `States`, `Mementos`, `CareTaker`, `SimulationJson`, `Rng`, klasy
wektorów). `Pages/Home.razor` to interfejs, a `wwwroot/canvas.js` – rysowanie i pobieranie plików. Diagram klas znajduje
się w `class-diagram.md`.
