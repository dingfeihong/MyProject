import numpy as np
import urllib.request
from sklearn import preprocessing
from sklearn import metrics


def iris_type(s):
	it = {b'Iris-setosa': 0, b'Iris-versicolor': 1, b'Iris-virginica': 2}
	it.setdefault('Iris-setosa')
	return it[s]
path = u'F:\资料\machine learning data\iris.data'
data = np.loadtxt(path, dtype=float, delimiter=',', converters={4: iris_type})


