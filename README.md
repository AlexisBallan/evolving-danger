# evolving-danger :

*English will follow*

### **Bienvenue sur Evolving Danger !**

Ce jeu met à l'honneur une intelligence artificielle évolutive. 
Vous commencez par affronter des IA nulle, faciles à vaincre. Mais **attention**, dès la fin de la première manche, seules les IA les plus performantes seront sélectionnées, reproduites et améliorées. 
À chaque manche, elles apprennent et s’adaptent, augmentant constamment leur niveau et leur ruse. Au début, vous pourrez sans doute dominer le terrain sans grande difficulté. Mais, au bout de quelques manches, la situation deviendra critique : vous serez traqué par des adversaires qui anticipent vos mouvements et exploitent vos faiblesses.

#### Essayez de survivre, mais les ennemis apprennent de vous. Combien de temps tiendrez-vous ?

Des fonctionnalités captivantes incluent des mécaniques de jeu en arène où les adversaires évoluent au fil du temps, un système de *sélection naturelle* simulant la survie des IA les plus adaptées, et des défis toujours renouvelés pour les joueurs qui tentent de rester un pas en avant de leurs ennemis intelligents.

### **Welcome to Evolving Danger !**

This game brings evolving artificial intelligence to life. 
You start each round facing fairly average, easy-to-defeat AI opponents. However, as each round ends, only the top-performing AIs will be selected, reproduced, and refined. 
With every new round, they learn and adapt, continually becoming more skilled and cunning. At first, you may find yourself dominating with ease. But after a few rounds, the challenge escalates: you'll be hunted by opponents who anticipate your moves and exploit your weaknesses.

#### Try to survive, but remember: the enemies are learning from you. How long can you last?

Exciting features include dynamic arena mechanics where adversaries evolve over time, a natural selection system simulating survival of the fittest AIs, and ever-renewed challenges for players striving to stay one step ahead of their intelligent foes.

# Détails techniques 

Petite explication de comment fonctionne les IA dans le jeu, c'est un mélange entre un algorithme génétique et un réseau de neurones (pour chaque IA), on va donc voir ce que fais l'algorithme génétique dans un premier temps pour ensuite voir ce que fais le réseau de neurones.

### Algorithme génétique 

La partie la plus simple des deux ! 
L'algorithme génétique sert principalement à sélectionner, parmis notre échentillons d'IA, lesquels on été les plus perfomant. Nous attribuons un système de point à l'IA pour ces actions (positif ou négatif), il faut trouver un **PARFAIT** équilibre entre les récompense et les malus de points, et surtout sélectionner minutieusement les comportements attendu pour les récompenser, c'est un travail qui prend du temps et beaucoup d'essaie erreur pour trouver la bonne combinaison. L'IA gagne des points quand elle frappe un de ses ennemies (pour rappel c'est un battle royal donc tout le monde est seul et les IA s'attaquent entre elles).

- Frapper un ennemie fais gagner des points, après quelques secondes si elle ne se fait pas toucher elle gagne un bonus de point.
- Si l'IA se fait toucher elle perd des points (admettont 50 points), mais si elle à attaquer dans les dernière secondes et qu'elle a touché, elle ne perdra que la moitié des points habituel histoire de ne pas trop sanctionner le fais de frapper, ça reste prioritaire.
- Si l'IA tue un ennemie, elle gagne beaucoup de points
- Si l'IA dash et qu'elle a frappé un ennemie il y a quelques secondes ou qu'elle va frapper un ennemie quelques seconde elle gagne des points
- Si l'ennemie est proche de notre IA et qu'il est dans l'état "attaque" (il y a un délais de 0.1 secondes environs avant de prendre les dégâts d'une attaque) et que l'IA dash et ne se prend pas de dégât, elle gagne autant de points que si elle le frappe
- Si l'IA lance un rocher et frappe un ennemie elle gagne des points
- Si l'IA reste collé à un mur elle perd des points
- Si l'IA ne bouge pas elle perd des points
- Si l'IA ne détecte personne pendant longtemps elle commence à perdre des points
- Si l'IA détecte quelqu'un et qu'elle ne fais rien pendant plusieurs seconde (pas de dash, d'attaque ou de lancer), elle perd des points
- Si l'IA meurs sans avoir infligé de dégât elle perd beaucoup de points

### Réseau de neurones

La partie sombre et obscure :X
Pour le réseau de neurones, il y a plusieurs entrée (la postitions et l'état des entité dans un rayon autour de lui)

Donc pour la couche d'entrée : 
- La vie
- Les 3 cooldown (attaque normal, attaque distance et dash)
- La position ou l'entité est
- Les murs et obstacle
- Est-ce que l'ennemi lance une attaque

L'IA aura 2 couche caché (la première de 128 neurones et la deuxième de 64 neurones) avec activation ReLU. Étant donné le nombre de paramètre et la compléxité de prendre une bonne décision, c'est ce qui a été retenu.
Couche de sortie : Probabilités pour chaque action (déplacement, attaque, dash, lancer), avec une activation softmax pour les actions non directionnelles et une activation sigmoïde pour les actions directionnelles.
