import numpy as np
import random

from sklearn.metrics import accuracy_score
from sklearn import svm
from sklearn import datasets
from sklearn.naive_bayes import GaussianNB
from sklearn.cluster import k_means
from random import shuffle


def gc_opp_test(X_data,y_data,name,seed):
    list1_shuf = []
    list2_shuf = []
    index_shuf = range(len(X_data))
    shuffle(index_shuf,random = random.Random(seed).random)

    for i in index_shuf:
        list1_shuf.append(X_data[i])
        list2_shuf.append(y_data[i])

    y_data = list2_shuf
    X_data = list1_shuf

    split = len(X_data) / 2

    svc_poly = svm.SVC(kernel='poly')
    svc_poly.fit(X_data[split:],y_data[split:])
    print ("SVM Poly for " + name + ": " + str(100 * accuracy_score(y_data[0:split], svc_poly.predict(X_data[0:split]))))


    svc_rbf = svm.SVC(kernel='rbf')
    svc_rbf.fit(X_data[split:],y_data[split:])
    print ("SVM RBF for " + name + ": " + str(100 * accuracy_score(y_data[0:split], svc_rbf.predict(X_data[0:split]))))

    treec = GaussianNB()
    treec.fit(X_data, y_data)
    print ("Gaussian Naive Bayes for " + name + ": " + str(100 * accuracy_score(y_data[0:split], treec.predict(X_data[0:split]))))
    print(" ")


data = datasets.load_iris()
gc_opp_test(data.data,data.target,"Iris",4)

data = datasets.load_digits()
gc_opp_test(data.data,data.target,"Digit",4)


data = datasets.fetch_olivetti_faces()
gc_opp_test(data.data,data.target,"Olivetti Faces",0)












