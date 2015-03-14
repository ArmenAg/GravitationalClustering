# GravitationalClustering

Abstract
---------
The downfall of many supervised learning algorithms, such as neural networks, is the inherent need for a large amount of training data (Benediktsson et al., 1993). Although there is a lot of buzz about big data, there is still the problem of doing classiﬁcation from a small data-set. Other methods such as support vector machines, although capable of dealing with few samples, are inherently binary classiﬁers (Cortes and Vapnik, 1995), and are in need of learning strategies such as One vs All in the case of multi-classiﬁcation. In the presence of a large number of classes this can become problematic. In this paper we present, a novel approach to supervised learning through the method of clustering.UnliketraditionalmethodssuchasK-Means (MacQueen, 1967), Gravitational Clustering does not require the initial number of clusters, and automatically builds the clusters, individual samples can be arbitrarily weighted and it requires only few samples while staying resilient to over-ﬁtting. 

Introduction
------------
The name of this algorithm is derived from the metaphor that the algorithm was built upon. Each cluster is symbolic of a planet,and each planet has a mass and a radius as well as the class that it represents. But unlike real life planets, our planets are static with respect to other planets. The process of training can be conceptually thought of as building a universe. The process of predicting is simply placing a mass in the universe and tracing what planet it will appear on. This algorithm exhibits three nice properties: 
- Ability to learn from a few samples. 
- Ability to weight the importance of training vectors. 
- The nature of the algorithm makes it resilient to overﬁtting. 

The ability to weight the importance of training vectors as well as the ability to learn from a few samples allows us to model a system that supports the notion of prototypes, e.g. Eleanor Rosch (Lakoﬀ, 1987)(P. 41).

Notes
-----------
This is the repository behind the code that is presented in the Gravitational CLustering Algorithm Paper. The algorithm is written in C# but all the comparison algorithms are used via the sklearn library. 

Thank you for visiting.

Please feel free to send any questions to armen.ag@live.com
