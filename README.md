# AFS
 
Działanie programu: https://www.youtube.com/watch?v=f3c4kx3mKlI

Niestety moje obliczenia dotyczące przyszłej pozycji przeciwnika są błędne. Problem z obliczeniem występuje przez zmienna prędkość rigidbody, a nie tak jak założyłem w kodzie - prędkość stałą.
Do optymalizacji użyłem objectPooling. 
Stworzyłem również ScriptableObject ułatwiający dodawanie innych wież i wykorzystanie ich w grze.

Inne poprawki:
- GetComponent jest kosztowną operacją - zminimalizowałem jego używanie
- sqrMagnitude jest szybszą operacją niż magnitude - zostało to zanaczone w dokumentacji Unity
- rotacja wież jest płynna
- poparwa znajdowania najbliższego wroga przez wieże
- usunięto problem opisany w punkcie nr. 2
- usunięto zbędne warningi - 0414,0219,0649

- warning CS0414: The private field `___' is assigned but its value is never used
- warning CS0219: The variable `___' is assigned but its value is never used
- warning CS0649: "Field is never assigned"


Żółte element widoczne na filmie pojawiają się w mojej błednie wyliczonej przyszłej pozycji obiektu.
W kodzie zostały one usunięte.

Pełny czas jaki poświęciłem to 5h 18min - według mnie zdecydowanie za długo na to zadanie jednakże straciłem bardzo dużo czasu w próbie wyliczenia przyszłej pozycji.

Czas tworzenia poszczególnych feature'ów jest widoczny w commitach