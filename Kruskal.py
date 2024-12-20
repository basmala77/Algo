# Find function with path compression
def find(parent, node):
    if parent[node] != node:
        parent[node] = find(parent, parent[node])  # Path compression
    return parent[node]

# Union function with rank optimization
def union(parent, rank, u, v):
    root_u = find(parent, u)
    root_v = find(parent, v)

    if root_u != root_v:
        if rank[root_u] > rank[root_v]:
            parent[root_v] = root_u
        elif rank[root_u] < rank[root_v]:
            parent[root_u] = root_v
        else:
            parent[root_v] = root_u
            rank[root_u] += 1

# Kruskal's Algorithm
def kruskal(edges, n):
    edges.sort()  # Sort edges by weight
    parent = [i for i in range(n)]  # Initialize parent array
    rank = [0] * n  # Initialize rank array

    mst = []  # Store the edges of the MST
    total_weight = 0  # Total weight of the MST

    for weight, u, v in edges:
        if find(parent, u) != find(parent, v):  # Check for cycle
            union(parent, rank, u, v)  # Union the sets
            mst.append((u, v, weight))  # Add edge to MST
            total_weight += weight  # Update total weight

    return mst, total_weight

# Example usage
if __name__ == "__main__":
    # Example graph
    edges = [
        (1, 0, 1),  # (weight, vertex1, vertex2)
        (3, 0, 2),
        (4, 1, 2),
        (2, 1, 3),
        (5, 2, 3)
    ]
    n = 4  # Number of vertices

    mst, total_weight = kruskal(edges, n)
    print("Edges in MST:", mst)
    print("Total Weight of MST:", total_weight)
